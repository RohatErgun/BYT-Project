using Electronic_Store.Entities.Concrete;

namespace ElectronicStore.Tests
{
    public class ProductEntityTests
    {
        private class TestProductEntity : ProductEntity
        {
            public TestProductEntity(decimal price, string brand, string model, string color, string material)
                : base(price, brand, model, color, material)
            { }
        }

        [SetUp]
        public void Setup()
        {
            // Clear class extent before each test
            typeof(ProductEntity)
                .GetField("_productsExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new System.Collections.Generic.List<ProductEntity>());
        }

        [Test]
        public void Constructor_ShouldCreateProduct_WhenValidValues()
        {
            var product = new TestProductEntity(100, "BrandA", "ModelX", "Red", "Plastic");

            Assert.AreEqual(100, product.Price);
            Assert.AreEqual("BrandA", product.Brand);
            Assert.AreEqual("ModelX", product.Model);
            Assert.AreEqual("Red", product.Color);
            Assert.AreEqual("Plastic", product.Material);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenPriceNegative()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new TestProductEntity(-10, "BrandA", "ModelX", "Red", "Plastic");
            });
        }

        [Test]
        public void ClassExtent_ShouldAddProduct_WhenCreated()
        {
            var product = new TestProductEntity(50, "BrandB", "ModelY", "Blue", "Metal");
            CollectionAssert.Contains(ProductEntity.ProductsExtent, product);
        }

        [Test]
        public void Price_Setter_ShouldThrowException_WhenNegative()
        {
            var product = new TestProductEntity(10, "Brand", "Model", "Red", "Plastic");
            Assert.Throws<ArgumentException>(() => product.Price = -5);
        }
    }
}