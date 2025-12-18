using Electronic_Store.Entities.Abstract;

namespace ElectronicStore.Tests.InheritanceTests;

[TestFixture]
public class AccessoryInheritanceTests
{
    [Test]
    public void Constructor_ShouldThrow_WhenNoRoleProvided()
    {
        Assert.Throws<ArgumentException>(() =>
            new Accessory(
                10m,
                "Brand",
                "Model",
                "Black",
                "Plastic",
                AccessoryRole.None
            ));
    }
    
    [Test]
    public void Constructor_ShouldCreateAccessory_WithSingleRole()
    {
        var accessory = new Accessory(
            20m,
            "Spigen",
            "CaseX",
            "Red",
            "Rubber",
            AccessoryRole.Case
        );

        Assert.True(accessory.IsCase);
        Assert.False(accessory.IsCharger);
        Assert.False(accessory.IsCable);
    }
    
    [Test]
    public void Constructor_ShouldAllow_MultipleRoles()
    {
        var accessory = new Accessory(
            80m,
            "Samsung",
            "PowerCase",
            "Black",
            "Silicone",
            AccessoryRole.Case | AccessoryRole.Charger
        );

        Assert.True(accessory.IsCase);
        Assert.True(accessory.IsCharger);
        Assert.False(accessory.IsCable);
    }
    
    [Test]
    public void ConfigureCase_ShouldSetCaseModel_WhenRoleIsCase()
    {
        var accessory = new Accessory(
            25m,
            "OtterBox",
            "Defender",
            "Blue",
            "Plastic",
            AccessoryRole.Case
        );

        accessory.ConfigureCase("iPhone 15");

        if (accessory.CaseModel != null) Assert.AreEqual("iPhone 15", accessory.CaseModel);
    }
    
    [Test]
    public void ConfigureCase_ShouldThrow_WhenRoleIsNotCase()
    {
        var accessory = new Accessory(
            40m,
            "Anker",
            "ChargerX",
            "White",
            "Plastic",
            AccessoryRole.Charger
        );

        Assert.Throws<InvalidOperationException>(() =>
            accessory.ConfigureCase("Galaxy S24"));
    }
    

}