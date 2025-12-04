using Electronic_Store.Entities.Abstract;
using System;

namespace Electronic_Store.Entities.Concrete
{ 
    public class WorkerEntity : BaseEntity
    {
        private string _name;
        private string _surname;
        private string _position;
        private DateTime _startDate;
        private double _salary;
        private WorkerEntity? _managedBy;
        private HashSet<WorkerEntity> _listOfManagedWorkers = new HashSet<WorkerEntity>();
        public IReadOnlyCollection<WorkerEntity> ManagedWorkers => _listOfManagedWorkers;
        private DateTime? _endDate;
        public DepartmentEntity DepartmentEntity { get; private set; }

        private const double YearlyPromotionRate = 0.05;

        public WorkerEntity(string name, string surname, string position, DateTime startDate, double salary, WorkerEntity? managedBy)
        {
            Name = name;
            Surname = surname;
            Position = position;
            StartDate = startDate;
            Salary = salary;
            _endDate = null;
            ManagedBy = managedBy;
        }

        public WorkerEntity() { }
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

        public WorkerEntity? ManagedBy
        {
            get => _managedBy;
            set
            {
                if (_managedBy == value) return;

                _managedBy?._listOfManagedWorkers.Remove(this);

                if (value != null && value._listOfManagedWorkers.Count >= 5)
                {
                    throw new InvalidOperationException($"{value.Name} cannot manage more than 5 workers.");
                }

                _managedBy = value;

                if (_managedBy != null && !_managedBy._listOfManagedWorkers.Contains(this))
                {
                    _managedBy._listOfManagedWorkers.Add(this);
                }
            }
        }

        public void AddManagedWorker(WorkerEntity worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException(nameof(worker));
            }

            if (worker == this)
            {
                throw new InvalidOperationException("Worker cannot be managed by himself.");
            }
            if (_listOfManagedWorkers.Count >= 5)
            {
                throw new InvalidOperationException($"{Name} cannot manage more than 5 workers.");
            }

            if (_listOfManagedWorkers.Add(worker))
            {
               worker.ManagedBy = this;
            }
        }

        public void RemoveManagedWorker(WorkerEntity worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException(nameof(worker));
            }

            if (_listOfManagedWorkers.Remove(worker))
            {
                if (worker._managedBy == this)
                {
                    worker._managedBy = null;
                }
            }
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