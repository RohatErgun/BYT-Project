using Electronic_Store.Entities;

namespace Electronic_Store;

// Bag relationship create a list of reports in Worker class 
//  public List<Report> Reports { get; } = new List<Report>();

public class Worker : BaseEntity
{
    public string Name;
    public string Surname;
    public string position;
    public DateTime startDate;
    public DateTime? endDate;

    public double salary { get; set; }
    public double Salary
    {
        get => salary;
        private set => salary = value;
    }

    public Worker(string Name, string Surname, string position, DateTime startDate, double salary)
    {
        Name = this.Name;
        Surname = this.Surname;
        position = this.position;
        startDate = this.startDate;
        salary = this.salary;
        endDate = null;
    }
    
    // 1 to 1*
    public Department Department { get; internal set; } 

    public const double yearlyPromotionRate = 0.05;
    
    public void ApplyYearlyPromotion(){}
    
    public void trackEmployementDuration(){}
}
