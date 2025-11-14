namespace Electronic_Store.Entities;

using System.Collections.Generic;

public class Department
{
    public string address { get; set; }
    public string departmentName { get; set; }

    // 1 to 1* 
    private List<Worker> workers = new List<Worker>();

    public Department(string address, string departmentName)
    {
        address = this.address;
        departmentName = this.departmentName;
    }
}