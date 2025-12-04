using Electronic_Store.Entities.Concrete;

namespace ElectronicStore.Tests;

[TestFixture]
public class WorkerReflexiveAssociationTest
{   
    // 1 - ADDING MANAGER TO WORKER
    [Test]
    public void AddingWorkerToManagedWorkers_ShouldSetManagedByProperty()
    {
        var manager = new WorkerEntity("John", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        manager.AddManagedWorker(worker);

        CollectionAssert.Contains(manager.ManagedWorkers, worker);
        Assert.That(worker.ManagedBy, Is.SameAs(manager));
    }
    
    // 2 - REMOVING WORKER FROM MANAGER'S LIST
    [Test]
    public void RemovingMangedWorker_ShouldUnsetManagedBy()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);
        
        manager.RemoveManagedWorker(worker);
        CollectionAssert.DoesNotContain(manager.ManagedWorkers, worker);
        Assert.Null(worker.ManagedBy);
    }
    
    // 3 - ADDING MORE THAN 5 WORKERS THROWS AN EXCEPTION
    [Test]
    public void CannotAddMoreThanFiveManagedWorkers()
    {
        var manager = new WorkerEntity("Havier", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        for (int i = 0; i <= 4; i++)
        {
            var worker = new WorkerEntity($"Worker{i}", "Test", "Sales", DateTime.Now.AddYears(-1), 3000, null);
            manager.AddManagedWorker(worker);
        }
        Assert.That(manager.ManagedWorkers, Has.Count.EqualTo(5));
        var extraWorker = new WorkerEntity("Extra", "Test", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        Assert.Throws<InvalidOperationException>(() => manager.AddManagedWorker(extraWorker));
    }
    
    // 4 - ASSIGNING MANAGER TO YOURSELF THROWS AN EXCEPTION
    [Test]
    public void CannotAssignWorkerToManageItself()
    {
        var worker = new WorkerEntity("Self", "Manager", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        Assert.Throws<InvalidOperationException>(() => worker.AddManagedWorker(worker));
    }
    
    // 5 - ADDING NULL MANAGER THROWS AN EXCEPTION
    [Test]
    public void CannotAddNullManagedWorker()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        Assert.Throws<ArgumentNullException>(() => manager.AddManagedWorker(null!));
    }
    
    // 6 - AVOIDANCE OF ADDING MANAGED WORKER TWICE
    [Test]
    public void DuplicateManagedWorkerShouldNotBeAddedAgain()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        manager.AddManagedWorker(worker);
        manager.AddManagedWorker(worker);

        Assert.That(manager.ManagedWorkers, Has.Count.EqualTo(1));
    }
    
    // 7 - REASSERTING MANAGED WORKER REMOVES THE WORKER FROM PREVIOUS MANAGER
    [Test]
    public void SettingManagedByToNewManagerUpdatesBothSides()
    {
        var manager1 = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var manager2 = new WorkerEntity("Eve", "Johnson", "Manager", DateTime.Now.AddYears(-3), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, manager1);

        worker.ManagedBy = manager2;

        CollectionAssert.DoesNotContain(manager1.ManagedWorkers, worker);
        CollectionAssert.Contains(manager2.ManagedWorkers, worker);
        Assert.That(worker.ManagedBy, !Is.EqualTo(manager1));
        Assert.That(worker.ManagedBy, Is.EqualTo(manager2));
    }
    
}