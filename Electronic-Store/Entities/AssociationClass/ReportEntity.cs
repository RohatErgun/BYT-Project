using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.AssociationClass
{
    using System;

    public class ReportEntity : BaseEntity
    {
        private DateTime _issuedDate;
        private string _managerName;
        private string _workerName;
        private string _title;
        
        public WorkerEntity Worker { get; private set; }
        

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

        public ReportEntity(WorkerEntity worker, string managerName, string title)
        {
            Worker = worker ?? throw new ArgumentNullException(nameof(worker));
            ManagerName = managerName;
            WorkerName = worker.Name + " " + worker.Surname;
            Title = title;
            IssuedDate = DateTime.Now;
            worker.AddReport(this);
        }
        
        public ReportEntity() { }

        public void GenerateReport() { }

        public void ViewReport() { }
    }
}