namespace ElectronicStore.Tests;

using Electronic_Store.Entities.Concrete;

[TestFixture]
public class WorkerDepAssociationTests
{
    // 1 - ADD CONNECTION
    [Test]
    public void AddWorker_ShouldCreateReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Jack", "Sparrow", "Seller", DateTime.Now.AddYears(-1), 5500, null);

        department.AddWorker(worker);

        Assert.Contains(worker, department.Workers.ToList());
        Assert.That(worker.DepartmentEntity, Is.EqualTo(department));
    }

    // 2 - ASSIGN CONNECTION
    [Test]
    public void AssignDepartment_ShouldCreateReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Jane", "Austin", "Cashier", DateTime.Now.AddYears(-2), 5000, null);
    
        worker.AssignDepartment(department);
    
        Assert.That(worker.DepartmentEntity, Is.EqualTo(department));
        Assert.Contains(worker, department.Workers.ToList());
    }
    
    // 3 - NO DUPLICATES
    [Test]
    public void AddWorker_ShouldNotAllowDuplicates()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Emily", "Blunt", "Manager", DateTime.Now.AddYears(-3), 11000, null);
    
        department.AddWorker(worker);
        department.AddWorker(worker);
    
        Assert.That(department.Workers.Count, Is.EqualTo(1));
        Assert.That(worker.DepartmentEntity, Is.EqualTo(department));
    }
    
    // 4 - REMOVE LAST WORKER BLOCKED (1…* multiplicity)
    [Test]
    public void RemoveWorker_ShouldThrow_WhenDepartmentWouldHaveZeroWorkers()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Alice", "Inborderland", "Technician", DateTime.Now.AddYears(-1), 6000, null);
    
        department.AddWorker(worker);
    
        Assert.Throws<InvalidOperationException>(() =>
        {
            department.RemoveWorker(worker);
        });
    }
    
    // 5 - WORKER CANNOT REMOVE ITS OWN DEPARTMENT
    [Test]
    public void Worker_RemoveDepartment_ShouldThrowException()
    {
        var department = new DepartmentEntity("Main Street", "Electronics");
        var worker = new WorkerEntity("Bob", "Marley", "Support", DateTime.Now.AddYears(-1), 5000, null);
    
        department.AddWorker(worker);
    
        Assert.Throws<InvalidOperationException>(() =>
        {
            worker.RemoveDepartment();
        });
    }
    
    // 6 - SAME AS #4 (duplicate but harmless)
    [Test]
    public void RemoveWorker_ShouldThrowException_WhenDepartmentWouldHaveNoWorkersLeft()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Sarah Jessica", "Parker", "Supervisor", DateTime.Now.AddYears(-3), 8000, null);
    
        department.AddWorker(worker);
    
        Assert.Throws<InvalidOperationException>(() =>
        {
            department.RemoveWorker(worker);
        });
    }
    
    // 7 - EDIT CONNECTION (Worker → different Department)
    [Test]
    public void AssignDepartment_ShouldMoveWorkerFromOldDepartmentToNewDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");
    
        var worker = new WorkerEntity("Dwayne", "Johnson", "Technician", DateTime.Now.AddYears(-2), 6000, null);
    
        deptA.AddWorker(worker);
    
        worker.AssignDepartment(deptB);
    
        Assert.IsFalse(deptA.Workers.Contains(worker));
        Assert.IsTrue(deptB.Workers.Contains(worker));
        Assert.That(worker.DepartmentEntity, Is.EqualTo(deptB));
    }
    
    // 8 - EDIT CONNECTION (Department.AddWorker triggers move)
    [Test]
    public void AddWorker_ShouldMoveWorkerFromOldDepartment_WhenCalledOnNewDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");
    
        var worker = new WorkerEntity("Mark", "Ruffalo", "Engineer", DateTime.Now.AddYears(-3), 13000, null);
    
        deptA.AddWorker(worker);
        deptB.AddWorker(worker);
    
        Assert.IsFalse(deptA.Workers.Contains(worker));
        Assert.IsTrue(deptB.Workers.Contains(worker));
        Assert.That(worker.DepartmentEntity, Is.EqualTo(deptB));
    }
    
    // 9 - NULL ADD
    [Test]
    public void AddWorker_ShouldThrowException_WhenWorkerIsNull()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
    
        Assert.Throws<ArgumentNullException>(() =>
        {
            department.AddWorker(null!);
        });
    }
    
    // 10 - NULL ASSIGN
    [Test]
    public void AssignDepartment_ShouldThrowException_WhenDepartmentIsNull()
    {
        var worker = new WorkerEntity("John", "Wick", "Seller", DateTime.Now.AddYears(-1), 5000, null);
    
        Assert.Throws<ArgumentNullException>(() =>
        {
            worker.AssignDepartment(null!);
        });
    }
    
    // 11 - NULL REMOVE
    [Test]
    public void RemoveWorker_ShouldNotThrow_WhenWorkerIsNull()
    {
        var department = new DepartmentEntity("Main Street", "Electronics");
    
        Assert.DoesNotThrow(() =>
        {
            department.RemoveWorker(null!);
        });
    }
}