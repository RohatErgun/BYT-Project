using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class WorkerEntity : BaseEntity
    {
        private HashSet<WorkerEntity> _workers = new HashSet<WorkerEntity>();
        
        private string _name;
        private string _surname;
        private string _position;
        private DateTime _startDate;
        private double _salary;
        private DateTime? _endDate;

        public DepartmentEntity DepartmentEntity { get; internal set; }

        public const double YearlyPromotionRate = 0.05;

        public WorkerEntity(string name, string surname, string position, DateTime startDate, double salary)
        {
            Name = name;
            Surname = surname;
            Position = position;
            StartDate = startDate;
            Salary = salary;
            _endDate = null;
        }

        public WorkerEntity() { }

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Name cannot be empty.") : value;
        }

        public string Surname
        {
            get => _surname;
            set => _surname = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Surname cannot be empty.") : value;
        }

        public string Position
        {
            get => _position;
            set => _position = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Position cannot be empty.") : value;
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
