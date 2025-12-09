using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{ 
    public class WorkerEntity : BaseEntity
    {
        private string _name;
        private string _surname;
        private string _position;
        private DateTime _startDate;
        private double _salary;
        private DateTime? _endDate;
        private WorkerEntity? _manager;
        private readonly HashSet<WorkerEntity> _subordinates = new();
        public IReadOnlyCollection<WorkerEntity> Subordinates => _subordinates;
        public DepartmentEntity DepartmentEntity { get; private set; }

        private const double YearlyPromotionRate = 0.05;
        private const int MaxSubordinates = 5;

        public WorkerEntity(string name, string surname, string position, DateTime startDate, double salary, WorkerEntity? manager)
        {
            Name = name;
            Surname = surname;
            Position = position;
            StartDate = startDate;
            Salary = salary;
            _endDate = null;
            Manager = manager;
        }

        public void AssignManager(WorkerEntity manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (manager == this) throw new ArgumentException("The worker cannot manage himself.");
            if (Manager == manager) return;
            
            RemoveManager();
            
            Manager = manager;

            manager.AddSubordinate(this);
        }

        public void AddSubordinate(WorkerEntity subordinate)
        {
            if (subordinate == null) throw new ArgumentNullException(nameof(subordinate));
            if (_subordinates.Contains(subordinate)) return;
            if (_subordinates.Count >= MaxSubordinates) throw new InvalidOperationException($"{Name} cannot have more than {MaxSubordinates} subordinates.");
            if (subordinate == this) throw new InvalidOperationException("The worker cannot manage himself.");
            
            _subordinates.Add(subordinate);
            
            subordinate.AssignManager(this);
        }

        public void RemoveManager()
        {
            if (Manager == null) return;
            var oldManager = Manager;
            Manager = null;

            if (oldManager._subordinates.Contains(this))
            {
                oldManager.RemoveSubordinate(this);
            }
        }

        public void RemoveSubordinate(WorkerEntity subordinate)
        {
            if (subordinate == null) throw new ArgumentNullException(nameof(subordinate));
            if (!_subordinates.Contains(subordinate)) return;
            
            _subordinates.Remove(subordinate);

            if (subordinate.Manager == this)
            {
                subordinate.RemoveManager();
            }
        }
        public void AssignDepartment(DepartmentEntity department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            if (DepartmentEntity == department)
                return;

            if (DepartmentEntity != null)
            {
                DepartmentEntity.InternalRemoveWorker(this);
            }

            DepartmentEntity = department;

            department.InternalAddWorker(this);
        }

        public void RemoveDepartment()
        {
            throw new InvalidOperationException(
                "Worker cannot remove its own Department. This action must be done by Department."
            );
        }
        public string Name
        {
            get => _name;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty");
                }
                _name = value;
            }
        }

        public string Surname
        {
            get => _surname;
            set => _surname = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Surname cannot be empty.")
                : value;
        }

        public string Position
        {
            get => _position;
            set => _position = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Position cannot be empty.")
                : value;
        }
        
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Start date cannot be in the future.");

                _startDate = value;
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (value.HasValue && value.Value < StartDate)
                    throw new ArgumentException("End date cannot be before start date.");

                _endDate = value;
            }
        }

        public double Salary
        {
            get => _salary;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Salary cannot be negative.");
                _salary = value;
            }
        }

        public WorkerEntity? Manager
        {
            get => _manager;
            set
            {
                if(value == _manager) return;
                if(value == this) throw new ArgumentException("The manager cannot manage himself.");
                
                _manager?.RemoveSubordinate(this);
                
                _manager = value;

                if (_manager != null && !_manager._subordinates.Contains(this))
                {
                    _manager.AddSubordinate(this);
                }
                
            }
        }
        public void ApplyYearlyPromotion()
        {
            Salary *= (1 + YearlyPromotionRate);
        }

        public TimeSpan TrackEmploymentDuration()
        {
            var end = EndDate ?? DateTime.Now;
            return end - StartDate;
        }
    }
}