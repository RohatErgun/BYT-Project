using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities
{
    [Serializable]
    public class WorkerEntity : BaseEntity
    {
        private static List<WorkerEntity> _workersExtent = new List<WorkerEntity>();
        public static IReadOnlyList<WorkerEntity> WorkersExtent => _workersExtent.AsReadOnly();

        private string _name;
        private string _surname;
        private string _position;
        private DateTime _startDate;
        private double _salary;
        private DateTime? _endDate;

        // Worker MUST belong to exactly 1 Department (1..1 multiplicity)
        public DepartmentEntity DepartmentEntity { get; private set; }

        // BAG association with Report – left unchanged for now
        public List<ReportEntity> Reports { get; } = new List<ReportEntity>();

        public const double YearlyPromotionRate = 0.05;

        public WorkerEntity(string name, string surname, string position, DateTime startDate, double salary)
        {
            Name = name;
            Surname = surname;
            Position = position;
            StartDate = startDate;
            Salary = salary;
            _endDate = null;

            AddWorkerToExtent(this);
        }

        public WorkerEntity() 
        {
            AddWorkerToExtent(this);
        }

        // -----------------------------
        //   PROPERTIES
        // -----------------------------
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
            throw new InvalidOperationException("A Worker must always belong to exactly one Department.");
        }
        private static void AddWorkerToExtent(WorkerEntity worker)
        {
            if (worker == null)
                throw new ArgumentException("Worker cannot be null.");

            _workersExtent.Add(worker);
        }

        public static void SaveExtent(string path = "workers.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(List<WorkerEntity>));
                serializer.Serialize(file, _workersExtent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving workers extent: " + ex.Message);
            }
        }

        public static bool LoadExtent(string path = "workers.xml")
        {
            try
            {
                if (!File.Exists(path))
                {
                    _workersExtent.Clear();
                    return false;
                }

                using StreamReader file = File.OpenText(path);
                XmlSerializer serializer = new XmlSerializer(typeof(List<WorkerEntity>));
                _workersExtent = (List<WorkerEntity>)serializer.Deserialize(file)
                    ?? new List<WorkerEntity>();
            }
            catch
            {
                _workersExtent.Clear();
                return false;
            }

            return true;
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