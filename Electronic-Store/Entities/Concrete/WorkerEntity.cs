using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.AssociationClass;

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
        
        public void AssignDepartment(DepartmentEntity newDepartment)
        {
            if (newDepartment == null)
                throw new ArgumentNullException(nameof(newDepartment));

            // EXIT GUARD — prevents infinite loop
            if (DepartmentEntity == newDepartment)
                return;

            var oldDepartment = DepartmentEntity;

            // update reference
            DepartmentEntity = newDepartment;

            // remove from old
            if (oldDepartment != null && oldDepartment.Workers.Contains(this))
            {
                oldDepartment.RemoveWorker(this);
            }

            // reverse connection (public → public)
            if (!newDepartment.Workers.Contains(this))
            {
                newDepartment.AddWorker(this);
            }
        }

        public void RemoveDepartment()
        {
            DepartmentEntity = null;
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

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Name cannot be empty.")
                : value;
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
                if (value == _manager) return;
                if (value == this) throw new ArgumentException("The manager cannot manage himself.");

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
        private readonly List<ReportEntity> _reports = new(); 
        public IReadOnlyList<ReportEntity> Reports => _reports.AsReadOnly();
        
        public void AddReport(ReportEntity report)
        {
            if (report == null) 
                throw new ArgumentNullException(nameof(report));
            if (! _reports.Contains(report))
                _reports.Add(report);
        }

        public void RemoveReport(ReportEntity report)
        {
            if (report == null)
                throw new ArgumentNullException(nameof(report));

            _reports.Remove(report);
        }
        
        private readonly HashSet<ProductEntity> _products = new();
        public IReadOnlyCollection<ProductEntity> Products => _products;

        public void AddProduct(ProductEntity product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (_products.Contains(product))
            {
                return;
            }
            _products.Add(product);

            if (product.AddedBy != this)
            {
                product.AssignWorker(this);
            }
        }

        public void RemoveProduct(ProductEntity product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (!_products.Contains(product))
            {
                return;
            }
            
            _products.Remove(product);

            if (product.AddedBy == this)
            {
                product.ReassignWorker();
            }
        }
        
    }
}