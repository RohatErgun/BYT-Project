namespace Electronic_Store.Entities
{
    using System;

    [Serializable]
    public class Report : BaseEntity
    {
        private DateTime _issuedDate;
        private string _managerName;
        private string _workerName;
        private string _title;

        public DateTime IssuedDate
        {
            get => _issuedDate;
            set => _issuedDate = value;
        }

        public string ManagerName
        {
            get => _managerName;
            set => _managerName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Manager name cannot be empty.")
                : value;
        }

        public string WorkerName
        {
            get => _workerName;
            set => _workerName = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Worker name cannot be empty.")
                : value;
        }

        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Title cannot be empty.")
                : value;
        }

        public Report(string managerName, string workerName, string title)
        {
            ManagerName = managerName;
            WorkerName = workerName;
            Title = title;
            _issuedDate = DateTime.Now;
        }

        public Report() { }

        public void GenerateReport() { }

        public void ViewReport() { }
    }
}