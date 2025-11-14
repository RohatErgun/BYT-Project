namespace Electronic_Store.Entities;

public class Report
{
    public DateTime issuedDate;
    public string ManagerName;
    public string WorkerName;
    public string title;

    public Report(string ManagerName, string WorkerName, string title)
    {
        ManagerName = this.ManagerName;
        WorkerName = this.WorkerName;
        title = this.title;
    }
    
    public void GenerateReport(){}
    public void viewReport(){}
 
    // Bag relationship create a list of reports in Worker class   
}