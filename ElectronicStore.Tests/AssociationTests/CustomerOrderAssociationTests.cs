using Electronic_Store.Entities.ComplexAttributes;
using Electronic_Store.Entities.Concrete;
using DateTime = System.DateTime;

namespace ElectronicStore.Tests.AssociationTests;

[TestFixture]
public class CustomerOrderAssociationTests
{   
    private CustomerEntity CreateCustomer()
    {
        return new CustomerEntity(
            name: "John",
            surname: "Doe",
            email: ["john.doe@test.com"],
            address: new AddressAttribute("Street 1", "City", "00-000", 32, "Zip-Code"),
            phoneNumber: "123456789",
            birthDate: new DateTime(2005, 11, 04),
            studentCard: null,
            loyaltyAccount: null
        );
    }
    
    private OrderEntity CreateOrder()
    {
        return new OrderEntity(
            date: DateTime.Now,
            status: OrderEntity.OrderStatus.OnTheWay,
            paymentMethod: OrderEntity.PaymentMethodType.DebitCard
        );
    }

    // 1 - ADD FROM CUSTOMER SIDE
    [Test]
    public void AddingOrderFromCustomerSide_UpdatesBothSides()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        customer.AddOrder(order);

        CollectionAssert.Contains(customer.Orders, order);
        Assert.That(order.Customer, Is.SameAs(customer));
    }

    // 2 - ADD FROM ORDER SIDE
    [Test]
    public void AddingCustomerFromOrderSide_UpdatesBothSides()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        order.AssignCustomer(customer);

        CollectionAssert.Contains(customer.Orders, order);
        Assert.That(order.Customer, Is.SameAs(customer));
    }

    // 3 - REMOVE FROM CUSTOMER SIDE
    [Test]
    public void RemovingOrderFromCustomerSide_UpdatesBothSides()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        customer.AddOrder(order);
        customer.RemoveOrder(order);

        CollectionAssert.DoesNotContain(customer.Orders, order);
        Assert.That(order.Customer, Is.Null);
    }

    // 4 - REMOVE FROM ORDER SIDE
    [Test]
    public void RemovingCustomerFromOrderSide_UpdatesBothSides()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        order.AssignCustomer(customer);
        order.RemoveCustomer();

        CollectionAssert.DoesNotContain(customer.Orders, order);
        Assert.That(order.Customer, Is.Null);
    }

    // 5 - DUPLICATE ADD FROM CUSTOMER SIDE DOES NOT DUPLICATE
    [Test]
    public void AddingSameOrderTwice_FromCustomerSide_DoesNotDuplicate()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        customer.AddOrder(order);
        customer.AddOrder(order);

        Assert.That(customer.Orders, Has.Count.EqualTo(1));
        Assert.That(order.Customer, Is.SameAs(customer));
    }

    // 6 -DUPLICATE ADD FROM ORDER SIDE DOES NOT DUPLICATE
    [Test]
    public void AssigningSameCustomerTwice_FromOrderSide_DoesNotDuplicate()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        order.AssignCustomer(customer);
        order.AssignCustomer(customer);

        Assert.That(customer.Orders, Has.Count.EqualTo(1));
        Assert.That(order.Customer, Is.SameAs(customer));
    }

    // 7 - REMOVE NON-EXISTING RELATION DOES NOTHING
    [Test]
    public void RemovingNonExistingRelation_DoesNotThrow()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        Assert.DoesNotThrow(() => customer.RemoveOrder(order));
        Assert.DoesNotThrow(() => order.RemoveCustomer());

        Assert.That(customer.Orders, Is.Empty);
        Assert.That(order.Customer, Is.Null);
    }

    // 8 - RE-ADDING AFTER REMOVAL WORKS
    [Test]
    public void ReAddingOrderAfterRemoval_WorksCorrectly()
    {
        var customer = CreateCustomer();
        var order = CreateOrder();

        customer.AddOrder(order);
        customer.RemoveOrder(order);
        customer.AddOrder(order);

        CollectionAssert.Contains(customer.Orders, order);
        Assert.That(order.Customer, Is.SameAs(customer));
    }
    
}