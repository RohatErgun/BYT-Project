using Electronic_Store.Entities.Concrete;
using Electronic_Store.Entities.Abstract;

namespace ElectronicStore.Tests
{
    [TestFixture]
    public class PromotionProductAssociationTests
    {
        private class TestProduct : ProductEntity
        {
            public TestProduct(decimal price, string brand, string model, string color, string material)
                : base(price, brand, model, color, material) { }
        }

        // Promotion AddProduct 
        [Test]
        public void AddProduct_ShouldSetReversePromotionOnProduct()
        {
            var promo = new PromotionEntity(10, "Seasonal", "Summer sale");
            var product = new TestProduct(1000, "Apple", "iPhone", "Black", "Metal");

            promo.AddProduct(product);

            Assert.Contains(product, promo.Products.ToList());
            Assert.That(product.Promotion, Is.EqualTo(promo));
        }

        // Product.AssignPromotion 
        [Test]
        public void AssignPromotion_ShouldAddProductToPromotion()
        {
            var promo = new PromotionEntity(15, "Holiday", "Christmas sale");
            var product = new TestProduct(800, "Samsung", "Galaxy", "Blue", "Plastic");

            product.AssignPromotion(promo);

            Assert.That(product.Promotion, Is.EqualTo(promo));
            Assert.Contains(product, promo.Products.ToList());
        }

        // Duplicate blocked
        [Test]
        public void AddProduct_ShouldNotAddDuplicates()
        {
            var promo = new PromotionEntity(20, "Clearance", "Warehouse cleanout");
            var product = new TestProduct(500, "Sony", "Camera", "Silver", "Metal");

            promo.AddProduct(product);
            promo.AddProduct(product);

            Assert.That(promo.Products.Count, Is.EqualTo(1));
        }

        // RemoveProduct 
        [Test]
        public void RemoveProduct_ShouldRemoveReverseConnection()
        {
            var promo = new PromotionEntity(10, "Promo", "Basic discount");
            var product = new TestProduct(1200, "Dell", "Laptop", "Gray", "Aluminum");

            promo.AddProduct(product);
            promo.RemoveProduct(product);

            Assert.IsFalse(promo.Products.Contains(product));
            Assert.IsNull(product.Promotion);
        }

        // AssignPromotion should move product between promotions
        [Test]
        public void AssignPromotion_ShouldMoveProductToAnotherPromotion()
        {
            var promoA = new PromotionEntity(10, "A", "Promo A");
            var promoB = new PromotionEntity(20, "B", "Promo B");

            var product = new TestProduct(1500, "HP", "Pavilion", "Silver", "Metal");

            promoA.AddProduct(product);
            product.AssignPromotion(promoB);

            Assert.IsFalse(promoA.Products.Contains(product));
            Assert.IsTrue(promoB.Products.Contains(product));
            Assert.That(product.Promotion, Is.EqualTo(promoB));
        }

        // Null checks
        [Test]
        public void AddProduct_And_AssignPromotion_ShouldThrow_WhenNull()
        {
            var promo = new PromotionEntity(10, "NullTest", "Checking null");
            var product = new TestProduct(999, "Test", "T1", "Red", "Plastic");

            Assert.Throws<ArgumentNullException>(() => promo.AddProduct(null!));
            Assert.Throws<ArgumentNullException>(() => product.AssignPromotion(null!));
        }
    }
}