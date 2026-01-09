using Electronic_Store.Entities.Abstract;

namespace ElectronicStore.Tests.InheritanceTests;

[TestFixture]
public class MultiAspectProductTests
{
    private class TestProduct : ProductEntity
    {
        public TestProduct(
            decimal price,
            string brand,
            string model,
            string color,
            string material,
            ProductCondition condition = ProductCondition.New)
            : base(
                price,
                brand,
                model,
                color,
                material,
                condition,
                newInfo: condition == ProductCondition.New ? new NewProductInfo(DateTime.UtcNow, TimeSpan.FromDays(365)) : null,
                refurbishedInfo: condition == ProductCondition.Refurbished ? new RefurbishedProductInfo() : null)
        { }
    }
    private class TestProductInvalid : ProductEntity
    {
        public TestProductInvalid(
            decimal price,
            string brand,
            string model,
            string color,
            string material,
            ProductCondition condition,
            NewProductInfo? newInfo = null,
            RefurbishedProductInfo? refurbishedInfo = null)
            : base(price, brand, model, color, material, condition, newInfo, refurbishedInfo)
        { }
    }
    
       [Test]
        public void NewProduct_ShouldHaveConditionNew_AndNewInfo()
        {
            var product = new TestProduct(100, "BrandA", "ModelA", "Black", "Plastic", ProductCondition.New);

            Assert.AreEqual(ProductCondition.New, product.Condition);
            Assert.NotNull(product.NewInfo);
            Assert.Null(product.RefurbishedInfo);
        }

        [Test]
        public void RefurbishedProduct_ShouldHaveConditionRefurbished_AndRefurbishedInfo()
        {
            var product = new TestProduct(200, "BrandB", "ModelB", "White", "Metal", ProductCondition.Refurbished);

            Assert.AreEqual(ProductCondition.Refurbished, product.Condition);
            Assert.Null(product.NewInfo);
            Assert.NotNull(product.RefurbishedInfo);
            Assert.AreEqual(0.10m, product.RefurbishedInfo!.DiscountPercentage);
        }

        [Test]
        public void GetEffectivePrice_ShouldApplyDiscountForRefurbishedProduct()
        {
            var product = new TestProduct(1000, "BrandC", "ModelC", "Gray", "Metal", ProductCondition.Refurbished);

            Assert.AreEqual(900m, product.GetEffectivePrice());
        }

        [Test]
        public void GetEffectivePrice_ShouldReturnSamePriceForNewProduct()
        {
            var product = new TestProduct(500, "BrandD", "ModelD", "Blue", "Plastic", ProductCondition.New);

            Assert.AreEqual(500m, product.GetEffectivePrice());
        }

        [Test]
        public void Constructor_ShouldThrow_WhenNewProductInfoMissing()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new TestProductInvalid(
                    price: 100,
                    brand: "BrandX",
                    model: "ModelX",
                    color: "Red",
                    material: "Metal",
                    condition: ProductCondition.New,
                    newInfo: null,
                    refurbishedInfo: null
                );
            });
        }

        [Test]
        public void Constructor_ShouldThrow_WhenRefurbishedInfoMissing()
        {
            Assert.Throws<ArgumentException>(() =>
                new TestProductInvalid(100, "BrandX", "ModelX", "Red", "Metal", ProductCondition.Refurbished, refurbishedInfo: null)
            );
        }

        [Test]
        public void Condition_ShouldBeImmutableAfterConstruction()
        {
            var product = new TestProduct(150, "BrandZ", "ModelZ", "Green", "Plastic", ProductCondition.New);

            Assert.AreEqual(ProductCondition.New, product.Condition);
        }

        [Test]
        public void NewProductInfo_ShouldHaveValidManufacturingDateAndWarranty()
        {
            var product = new TestProduct(120, "BrandE", "ModelE", "Black", "Plastic", ProductCondition.New);

            Assert.That(product.NewInfo!.ManufacturingDate, Is.LessThanOrEqualTo(DateTime.UtcNow));
            Assert.That(product.NewInfo.WarrantyPeriod.TotalDays, Is.EqualTo(365).Within(1));
        } 
}

