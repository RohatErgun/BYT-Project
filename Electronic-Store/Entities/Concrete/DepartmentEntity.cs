using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    using System.Collections.Generic;
    
    [Serializable]
    public class DepartmentEntity : BaseEntity
    {
        private string _address;
        private string _departmentName;
        private List<WorkerEntity> _workers = new List<WorkerEntity>();

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

        public IReadOnlyList<WorkerEntity> Workers => _workers.AsReadOnly();

        public DepartmentEntity(string address, string departmentName)
        {
            Address = address;
            DepartmentName = departmentName;
        }

        public DepartmentEntity() { }

        public void AddWorker(WorkerEntity workerEntity)
        {
            if (workerEntity == null)
                throw new ArgumentException("Worker cannot be null.");

            if (!_workers.Contains(workerEntity))
            {
                _workers.Add(workerEntity);
                workerEntity.DepartmentEntity = this;
            }
        }
    }
}