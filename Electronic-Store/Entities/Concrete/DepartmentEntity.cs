using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class DepartmentEntity : BaseEntity
    {
        private string _address;
        private string _departmentName;

        private readonly HashSet<WorkerEntity> _workers = new();

        public string Address
        {
            get => _address;
            set => _address = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Address cannot be empty.")
                : value;
        }

        public string DepartmentName
        {
            get => _departmentName;
            set => _departmentName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Department name cannot be empty.")
                : value;
        }

        public IReadOnlyCollection<WorkerEntity> Workers => _workers;

        public DepartmentEntity(string address, string departmentName)
        {
            Address = address;
            DepartmentName = departmentName;
        }

        public DepartmentEntity() { }

       
        public void AddWorker(WorkerEntity worker)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker));

            // EXIT GUARD
            if (_workers.Contains(worker))
                return;

            // add to list
            _workers.Add(worker);

            // reverse update (public → public)
            if (worker.DepartmentEntity != this)
            {
                worker.AssignDepartment(this);
            }
        }

        public void RemoveWorker(WorkerEntity worker)
        {
            if (worker == null)
                return;

            if (!_workers.Contains(worker))
                return;

            // UML: 1..* 
            if (_workers.Count == 1)
                throw new InvalidOperationException("A Department must have at least one Worker.");

            _workers.Remove(worker);

            if (worker.DepartmentEntity == this)
            {
                worker.RemoveDepartment();
            }
        }
    }
}
