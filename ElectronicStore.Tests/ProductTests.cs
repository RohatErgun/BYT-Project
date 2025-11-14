using System;
using Electronic_Store.Entities;
using NUnit.Framework;

namespace ElectronicStore.Tests
{
    public class ProductTests
    {
        private class TestProduct : Product
        {
            public TestProduct(decimal price, string brand, string model, string color, string material)
                : base(price, brand, model, color, material)
            { }
        }

        [SetUp]
        public void Setup()
        {
            // Clear class extent before each test
            typeof(Product)
                .GetField("_productsExtent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .SetValue(null, new System.Collections.Generic.List<Product>());
        }

        [Test]
        public void Constructor_ShouldCreateProduct_WhenValidValues()
        {
            var product = new TestProduct(100, "BrandA", "ModelX", "Red", "Plastic");

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
                new TestProduct(-10, "BrandA", "ModelX", "Red", "Plastic");
            });
        }

        [Test]
        public void ClassExtent_ShouldAddProduct_WhenCreated()
        {
            var product = new TestProduct(50, "BrandB", "ModelY", "Blue", "Metal");
            CollectionAssert.Contains(Product.ProductsExtent, product);
        }

        [Test]
        public void Price_Setter_ShouldThrowException_WhenNegative()
        {
            var product = new TestProduct(10, "Brand", "Model", "Red", "Plastic");
            Assert.Throws<ArgumentException>(() => product.Price = -5);
        }
    }
}