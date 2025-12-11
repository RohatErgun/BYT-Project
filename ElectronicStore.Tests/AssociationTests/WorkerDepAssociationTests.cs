namespace ElectronicStore.Tests;

using Electronic_Store.Entities.Concrete;

[TestFixture]
public class WorkerDepAssociationTests
{
    // ADD CONNECTION 
    [Test]
    public void AddWorker_ShouldCreateReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Jack", "Sparrow", "Seller",
            DateTime.Now.AddYears(-1), 5500, null);

        department.AddWorker(worker);

        Assert.Contains(worker, department.Workers.ToList());
        Assert.That(worker.DepartmentEntity, Is.EqualTo(department));
    }

    // ASSIGN CONNECTION 
    [Test]
    public void AssignDepartment_ShouldCreateReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Jane", "Austin", "Cashier",
            DateTime.Now.AddYears(-2), 5000, null);

        worker.AssignDepartment(department);

        Assert.That(worker.DepartmentEntity, Is.EqualTo(department));
        Assert.Contains(worker, department.Workers.ToList());
    }

    // DUPLICATE ADD BLOCKED
    [Test]
    public void AddWorker_ShouldNotAllowDuplicates()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Emily", "Blunt", "Manager",
            DateTime.Now.AddYears(-3), 11000, null);

        department.AddWorker(worker);
        department.AddWorker(worker); // duplicate attempt

        Assert.That(department.Workers.Count, Is.EqualTo(1));
    }

    // REMOVE CONNECTION
    [Test]
    public void RemoveWorker_ShouldRemoveReverseConnection()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Alice", "Inborderland", "Technician",
            DateTime.Now.AddYears(-1), 6000, null);

        department.AddWorker(worker);
        department.AddWorker(new WorkerEntity("Extra", "Person", "Temp",
            DateTime.Now, 1000, null));

        department.RemoveWorker(worker);

        Assert.IsFalse(department.Workers.Contains(worker));
        Assert.IsNull(worker.DepartmentEntity);
    }

    // MULTIPLICITY
    [Test]
    public void RemoveWorker_ShouldThrow_WhenLastWorker()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("Sarah Jessica", "Parker", "Supervisor",
            DateTime.Now.AddYears(-3), 8000, null);

        department.AddWorker(worker);

        Assert.Throws<InvalidOperationException>(() =>
        {
            department.RemoveWorker(worker);
        });
    }

    // UPDATE CONNECTION 
    [Test]
    public void AssignDepartment_ShouldMoveWorkerToAnotherDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");

        var worker = new WorkerEntity("Dwayne", "Johnson", "Technician",
            DateTime.Now.AddYears(-2), 6000, null);

        deptA.AddWorker(worker);
        deptA.AddWorker(new WorkerEntity("Extra", "Worker", "Temp",
            DateTime.Now, 1000, null));

        worker.AssignDepartment(deptB);

        Assert.IsFalse(deptA.Workers.Contains(worker));
        Assert.IsTrue(deptB.Workers.Contains(worker));
        Assert.That(worker.DepartmentEntity, Is.EqualTo(deptB));
    }

    // UPDATE CONNECTION 
    [Test]
    public void AddWorker_ShouldMoveWorkerFromPreviousDepartment()
    {
        var deptA = new DepartmentEntity("Marszalkowska", "Electronics");
        var deptB = new DepartmentEntity("Nowy Swiat", "Computers");

        var worker = new WorkerEntity("Mark", "Ruffalo", "Engineer",
            DateTime.Now.AddYears(-3), 13000, null);

        deptA.AddWorker(worker);
        deptA.AddWorker(new WorkerEntity("ExtraGuy", "Temp", "Helper",
            DateTime.Now, 900, null));

        deptB.AddWorker(worker);

        Assert.IsFalse(deptA.Workers.Contains(worker));
        Assert.IsTrue(deptB.Workers.Contains(worker));
        Assert.That(worker.DepartmentEntity, Is.EqualTo(deptB));
    }

    // NULL ASSIGN AND ADD VALIDATIONS
    [Test]
    public void AddWorker_And_AssignDepartment_ShouldThrow_WhenNull()
    {
        var department = new DepartmentEntity("Centrum", "Electronics");
        var worker = new WorkerEntity("John", "Wick", "Seller",
            DateTime.Now.AddYears(-1), 5000, null);

        Assert.Throws<ArgumentNullException>(() => department.AddWorker(null!));
        Assert.Throws<ArgumentNullException>(() => worker.AssignDepartment(null!));
    }
}