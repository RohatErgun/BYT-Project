namespace ElectronicStore.Tests;

using Electronic_Store.Entities.Concrete;
using Electronic_Store.Entities;

public class WorkerDepAssociationTests
{
    [TestFixture]
    public class DepartmentWorkerTests
    {
        [Test]
        public void AddWorker_ShouldCreateReverseConnection()
        {
            var department = new DepartmentEntity("Centrum", "Electronics");
            var worker = new WorkerEntity("Jack", "Sparrow", "Seller", DateTime.Now.AddYears(-1), 5500);

            department.AddWorker(worker);

            Assert.Contains(worker, department.Workers.ToList());

            Assert.AreEqual(department, worker.DepartmentEntity);
        }
    }
    [Test]
    public void AssignDepartment_ShouldCreateReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Jane", "Austin", "Cashier", DateTime.Now.AddYears(-2), 5000);

        worker.AssignDepartment(department);
        
        Assert.AreEqual(department, worker.DepartmentEntity);

        Assert.Contains(worker, department.Workers.ToList());
    }
    [Test]
    public void AddWorker_ShouldNotAllowDuplicates()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Emily", "Blunt", "Manager", DateTime.Now.AddYears(-3), 11000);
    
        
        department.AddWorker(worker);
        department.AddWorker(worker); // duplicate attempt
        
        Assert.AreEqual(1, department.Workers.Count);

        Assert.AreEqual(department, worker.DepartmentEntity);
    }
    [Test]
    public void RemoveWorker_ShouldThrowException_WhenWorkerWouldHaveNoDepartment()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Alice", "Inborderland", "Technician", DateTime.Now.AddYears(-1), 6000);

        department.AddWorker(worker);

        Assert.Throws<InvalidOperationException>(() =>
        {
            department.RemoveWorker(worker);
        });
    }
    [Test]
    public void Worker_RemoveDepartment_ShouldThrowException()
    {
        var department = new DepartmentEntity("Main Street", "Electronics");
        var worker = new WorkerEntity("Bob", "Marley", "Support", DateTime.Now.AddYears(-1), 5000);

        department.AddWorker(worker);

        Assert.Throws<InvalidOperationException>(() =>
        {
            worker.RemoveDepartment();
        });
    }
    [Test]
    public void RemoveWorker_ShouldThrowException_WhenDepartmentWouldHaveNoWorkersLeft()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Sarah Jessica", "Parker", "Supervisor", DateTime.Now.AddYears(-3), 8000);

        department.AddWorker(worker);

        Assert.Throws<InvalidOperationException>(() =>
        {
            department.RemoveWorker(worker);
        });
    }
    [Test]
    public void AssignDepartment_ShouldMoveWorkerFromOldDepartmentToNewDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");

        var worker = new WorkerEntity("Dwayne", "Johnson", "Technician", DateTime.Now.AddYears(-2), 6000);

        deptA.AddWorker(worker);

        worker.AssignDepartment(deptB);

        Assert.IsFalse(deptA.Workers.Contains(worker));

        Assert.IsTrue(deptB.Workers.Contains(worker));

        Assert.AreEqual(deptB, worker.DepartmentEntity);
    }
    [Test]
    public void AddWorker_ShouldMoveWorkerFromOldDepartment_WhenCalledOnNewDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");

        var worker = new WorkerEntity("Mark", "Ruffalo", "Engineer", DateTime.Now.AddYears(-3), 13000);

        deptA.AddWorker(worker);

        deptB.AddWorker(worker);

        Assert.IsFalse(deptA.Workers.Contains(worker));

        Assert.IsTrue(deptB.Workers.Contains(worker));

        Assert.AreEqual(deptB, worker.DepartmentEntity);
    }
    [Test]
    public void AddWorker_ShouldThrowException_WhenWorkerIsNull()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");

        Assert.Throws<ArgumentNullException>(() =>
        {
            department.AddWorker(null);
        });
    }
    [Test]
    public void AssignDepartment_ShouldThrowException_WhenDepartmentIsNull()
    {
        var worker = new WorkerEntity("John", "Wick", "Seller", DateTime.Now.AddYears(-1), 5000);

        Assert.Throws<ArgumentNullException>(() =>
        {
            worker.AssignDepartment(null);
        });
    }
    [Test]
    public void RemoveWorker_ShouldNotThrow_WhenWorkerIsNull()
    {
        var department = new DepartmentEntity("Main Street", "Electronics");

        Assert.DoesNotThrow(() =>
        {
            department.RemoveWorker(null);
        });
    }
}