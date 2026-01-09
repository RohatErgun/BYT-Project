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
    public void Constructor_ShouldCreateAccessory_WithSingleRole_AndNoRoleData()
    {
        var accessory = new Accessory(
            20m,
            "Spigen",
            "CaseX",
            "Red",
            "Rubber",
            AccessoryRole.Case,
            caseInfo: new CaseInfo("iPhone 15")
        );

        Assert.True(accessory.IsCase);
        Assert.False(accessory.IsCharger);
        Assert.False(accessory.IsCable);

        Assert.NotNull(accessory.Case);
        Assert.AreEqual("iPhone 15", accessory.Case!.CaseModel);
    }

    [Test]
    public void Constructor_ShouldAllow_MultipleRoles_WithMatchingData()
    {
        var accessory = new Accessory(
            80m,
            "Samsung",
            "PowerCase",
            "Black",
            "Silicone",
            AccessoryRole.Case | AccessoryRole.Charger,
            caseInfo: new CaseInfo("Galaxy S24"),
            chargerInfo: new ChargerInfo(25)
        );

        Assert.True(accessory.IsCase);
        Assert.True(accessory.IsCharger);
        Assert.False(accessory.IsCable);

        Assert.NotNull(accessory.Case);
        Assert.NotNull(accessory.Charger);
        Assert.IsNull(accessory.Cable);
    }

    [Test]
    public void Constructor_ShouldThrow_WhenCaseRoleHasNoCaseInfo()
    {
        Assert.Throws<ArgumentException>(() =>
            new Accessory(
                25m,
                "OtterBox",
                "Defender",
                "Blue",
                "Plastic",
                AccessoryRole.Case
            ));
    }

    [Test]
    public void Constructor_ShouldThrow_WhenCaseInfoProvidedWithoutCaseRole()
    {
        Assert.Throws<ArgumentException>(() =>
            new Accessory(
                40m,
                "Anker",
                "ChargerX",
                "White",
                "Plastic",
                AccessoryRole.Charger,
                caseInfo: new CaseInfo("Galaxy S24")
            ));
    }

    [Test]
    public void Constructor_ShouldThrow_WhenChargerRoleHasNoChargerInfo()
    {
        Assert.Throws<ArgumentException>(() =>
            new Accessory(
                40m,
                "Anker",
                "ChargerX",
                "White",
                "Plastic",
                AccessoryRole.Charger
            ));
    }

    [Test]
    public void Constructor_ShouldThrow_WhenCableInfoProvidedWithoutCableRole()
    {
        Assert.Throws<ArgumentException>(() =>
            new Accessory(
                30m,
                "Brand",
                "CableX",
                "Black",
                "Plastic",
                AccessoryRole.Case,
                cableInfo: new CableInfo(1.5m, "USB-C")
            ));
    }
}