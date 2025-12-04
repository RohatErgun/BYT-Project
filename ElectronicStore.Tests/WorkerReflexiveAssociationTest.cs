using Electronic_Store.Entities.Concrete;

namespace ElectronicStore.Tests;

[TestFixture]
public class WorkerReflexiveAssociationTest
{
    [Test]
    public void AddingWorkerToManagedWorkers_ShouldSetManagedByProperty()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        manager.AddManagedWorker(worker);

        CollectionAssert.Contains(manager.ManagedWorkers, worker);
        Assert.AreSame(manager, worker.ManagedBy);
    }

    [Test]
    public void RemovingMangedWorker_ShouldUnsetManagedBy()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);
        
        manager.RemoveManagedWorker(worker);
        CollectionAssert.DoesNotContain(manager.ManagedWorkers, worker);
        Assert.Null(worker.ManagedBy);
    }
    
    [Test]
    public void CannotAddMoreThanFiveManagedWorkers()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        for (int i = 1; i <= 5; i++)
        {
            var worker = new WorkerEntity($"Worker{i}", "Test", "Sales", DateTime.Now.AddYears(-1), 3000, null);
            manager.AddManagedWorker(worker);
        }

        var extraWorker = new WorkerEntity("Extra", "Test", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        Assert.Throws<InvalidOperationException>(() => manager.AddManagedWorker(extraWorker));
    }
    
    [Test]
    public void CannotAssignWorkerToManageItself()
    {
        var worker = new WorkerEntity("Self", "Manager", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        Assert.Throws<InvalidOperationException>(() => worker.AddManagedWorker(worker));
    }
    
    [Test]
    public void CannotAddNullManagedWorker()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);

        Assert.Throws<ArgumentNullException>(() => manager.AddManagedWorker(null!));
    }
    
    [Test]
    public void DuplicateManagedWorkerShouldNotBeAddedAgain()
    {
        var manager = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, null);

        manager.AddManagedWorker(worker);
        manager.AddManagedWorker(worker);

        Assert.That(manager.ManagedWorkers, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void SettingManagedByToNewManagerUpdatesBothSides()
    {
        var manager1 = new WorkerEntity("Alice", "Smith", "Manager", DateTime.Now.AddYears(-2), 5000, null);
        var manager2 = new WorkerEntity("Eve", "Johnson", "Manager", DateTime.Now.AddYears(-3), 5000, null);
        var worker = new WorkerEntity("Bob", "Brown", "Sales", DateTime.Now.AddYears(-1), 3000, manager1);

        worker.ManagedBy = manager2;

        CollectionAssert.DoesNotContain(manager1.ManagedWorkers, worker);
        CollectionAssert.Contains(manager2.ManagedWorkers, worker);
        Assert.AreEqual(manager2, worker.ManagedBy);
    }
    
}