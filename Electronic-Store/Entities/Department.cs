namespace Electronic_Store.Entities
{
    using System.Collections.Generic;
    
    [Serializable]
    public class Department : BaseEntity
    {
        private string _address;
        private string _departmentName;
        private List<Worker> _workers = new List<Worker>();

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

        public IReadOnlyList<Worker> Workers => _workers.AsReadOnly();

        public Department(string address, string departmentName)
        {
            Address = address;
            DepartmentName = departmentName;
        }

        public Department() { }

        public void AddWorker(Worker worker)
        {
            if (worker == null)
                throw new ArgumentException("Worker cannot be null.");

            if (!_workers.Contains(worker))
            {
                _workers.Add(worker);
                worker.Department = this;
            }
        }
    }
}