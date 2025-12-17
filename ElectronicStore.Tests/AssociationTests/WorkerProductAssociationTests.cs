using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace ElectronicStore.Tests.AssociationTests;

[TestFixture]
public class WorkerProductAssociationTests
{
    private class TestProductEntity(
        decimal price, string brand, string model, string color, string material
        ) : ProductEntity(price, brand, model, color, material);
    
    //  1 - ADDING PRODUCT AS WORKER
    [Test]
    public void AddingProductAsWorker()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");
        
        worker.AddProduct(product);
        
        CollectionAssert.Contains(worker.Products, product);
        Assert.That(product.AddedBy, Is.SameAs(worker));
    }
    
    // 2 - ASSIGNING WORKER TO PRODUCT 
    [Test]
    public void AddingWorkerFromProductSide()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        product.AssignWorker(worker);

        CollectionAssert.Contains(worker.Products, product);
        Assert.That(product.AddedBy, Is.SameAs(worker));
    }
    
    // 3 - REMOVE PRODUCT AS WORKER 
    [Test]
    public void RemovingProductFromWorkerSide()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        worker.AddProduct(product);
        worker.RemoveProduct(product);

        CollectionAssert.DoesNotContain(worker.Products, product);
        Assert.That(product.AddedBy, Is.Null);
    }
    
    // 4 - REASSIGNING WORKER FROM PRODUCT
    [Test]
    public void RemovingWorkerFromProductSide()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        product.AssignWorker(worker);
        product.ReassignWorker();

        CollectionAssert.DoesNotContain(worker.Products, product);
        Assert.That(product.AddedBy, Is.Null);
    }
    
    // 5 - AVOIDING WORKER ADDED BY DUPLICATES
    [Test]
    public void AddingSameProductTwice_AsWorker_DoesNotDuplicate()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        worker.AddProduct(product);
        worker.AddProduct(product);

        Assert.That(worker.Products, Has.Count.EqualTo(1));
        Assert.That(product.AddedBy, Is.SameAs(worker));
    }
    
    // 6 - AVOIDING DUPLICATES TO ASSIGNING WORKER 
    [Test]
    public void AssigningSameWorkerTwice_AsProduct_DoesNotDuplicate()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        product.AssignWorker(worker);
        product.AssignWorker(worker);

        Assert.That(worker.Products, Has.Count.EqualTo(1));
        Assert.That(product.AddedBy, Is.SameAs(worker));
    }
    
    // 7 - ADDITION POSSIBLE AFTER DELETION
    [Test]
    public void ReAddingProductAfterRemoval()
    {
        var worker = new WorkerEntity("testName", "testSurname", "testPosition", DateTime.Now, 3200, null);
        var product = new TestProductEntity(123, "testBrand", "testModel", "testColor", "testMaterial");

        worker.AddProduct(product);
        worker.RemoveProduct(product);
        worker.AddProduct(product);

        CollectionAssert.Contains(worker.Products, product);
        Assert.That(product.AddedBy, Is.SameAs(worker));
    }
}