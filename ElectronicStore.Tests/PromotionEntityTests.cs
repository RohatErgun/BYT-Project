using Electronic_Store.Entities.Concrete;
namespace ElectronicStore.Tests
{
    public class PromotionEntityTests
    {
        [SetUp]
        public void Setup()
        {
            // Clear class extent before each test
            typeof(PromotionEntity)
                .GetField("_promotionsExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new System.Collections.Generic.List<PromotionEntity>());
        }

        [Test]
        public void Constructor_ShouldCreatePromotion_WhenValidValues()
        {
            var promo = new PromotionEntity(10, "Seasonal", "10% off for summer");

            Assert.AreEqual(10, promo.DiscountPercentage);
            Assert.AreEqual("Seasonal", promo.Type);
            Assert.AreEqual("10% off for summer", promo.Description);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenDiscountTooLow()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new PromotionEntity(1, "Seasonal", "Too low discount");
            });
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenDiscountTooHigh()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new PromotionEntity(60, "Seasonal", "Too high discount");
            });
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenTypeEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new PromotionEntity(10, "", "Some description");
            });
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenDescriptionEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new PromotionEntity(10, "Promo", "");
            });
        }

        [Test]
        public void ClassExtent_ShouldAddPromotion_WhenCreated()
        {
            var promo = new PromotionEntity(15, "Holiday", "15% off for holidays");
            CollectionAssert.Contains(PromotionEntity.PromotionsExtent, promo);
        }

        [Test]
        public void DiscountPercentage_Setter_ShouldThrowException_WhenOutOfRange()
        {
            var promo = new PromotionEntity(10, "Promo", "Description");
            Assert.Throws<ArgumentException>(() => promo.DiscountPercentage = 2);
            Assert.Throws<ArgumentException>(() => promo.DiscountPercentage = 55);
        }
    }
}
