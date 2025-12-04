using Electronic_Store.Entities.Abstract;
using System.Collections.Generic;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class DepartmentEntity : BaseEntity
    {
        private string _address;
        private string _departmentName;

        private HashSet<WorkerEntity> _workers = new HashSet<WorkerEntity>();

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

            if (_workers.Contains(worker))
                return;

            if (worker.DepartmentEntity != null && worker.DepartmentEntity != this)
            {
                worker.DepartmentEntity.InternalRemoveWorker(worker);
            }

            _workers.Add(worker);

            worker.AssignDepartment(this);
        }
        public void RemoveWorker(WorkerEntity worker)
        {
            if (worker == null)
                return;

            if (!_workers.Contains(worker))
                return;

            if (_workers.Count == 1)
                throw new InvalidOperationException(
                    "A Department must have at least one Worker (1..* multiplicity)."
                );

            _workers.Remove(worker);

            // worker.RemoveDepartment();
        }
        internal void InternalAddWorker(WorkerEntity worker)
        {
            if (worker == null)
                return;

            _workers.Add(worker);
        }

        internal void InternalRemoveWorker(WorkerEntity worker)
        {
            if (worker == null)
                return;

            _workers.Remove(worker);
        }
    }
}