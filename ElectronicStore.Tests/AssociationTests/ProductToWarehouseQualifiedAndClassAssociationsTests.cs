using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;
using Electronic_Store.Entities.AssociationClass;
using Electronic_Store.Entities.ComplexAttributes;

namespace ElectronicStore.Tests
{
    public class ProductToWarehouseQualifiedAndClassAssociationsTests
    {
        private class TestProductEntity : ProductEntity
        {
            public TestProductEntity(decimal price, string brand, string model, string color, string material)
                : base(price, brand, model, color, material)
            { }
        }
        
        private List<(string, double)> GetDefaultStorageRooms()
        {
            return new List<(string, double)>
            {
                ("Main Room", 500),
                ("Back Room", 300),
                ("Loading Dock", 400)
            };
        }
        
        private AddressAttribute CreateTestAddress()
        {
            return new AddressAttribute("USA", "New York", "5th Ave", 101, "10001");
        }

        [SetUp]
        public void Setup()
        {
            
        }

     
        // Association Establishment & Reverse Connections
      

        [Test]
        public void Constructor_ShouldCreateAssociation_AndLinkBothDirections()
        {
           
            var product = new TestProductEntity(100, "Sony", "ModelX", "Black", "Plastic");
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());
            int initialQuantity = 50;
            
            
            // The Association Class constructor does the linking
            var stock = new ProductStock(product, warehouse, initialQuantity);

           
            // Check Association Class Attributes
            Assert.AreEqual(initialQuantity, stock.Quantity);
            Assert.AreEqual(product, stock.Product);
            Assert.AreEqual(warehouse, stock.Warehouse);

            // Product -> Stock
            CollectionAssert.Contains(product.Stocks, stock, "Product should have reference to the stock.");

            // Warehouse -> Stock
            CollectionAssert.Contains(warehouse.Inventory, stock, "Warehouse should have reference to the stock.");
        }

        [Test]
        public void Quantity_Setter_ShouldModifyValue_AndBeVisibleFromBothSides()
        {
            
            var product = new TestProductEntity(100, "Sony", "ModelX", "Black", "Plastic");
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());
            var stock = new ProductStock(product, warehouse, 10);
            
            stock.Quantity = 25;
            
            var stockInWarehouse = warehouse.Inventory.FirstOrDefault(s => s == stock);
            Assert.IsNotNull(stockInWarehouse);
            Assert.AreEqual(25, stockInWarehouse.Quantity);
        }
        
        
        // Qualified Association Logic
        

        [Test]
        public void GetStockByQualifiers_ShouldRetrieveCorrectStock()
        {
          
            string brand = "Apple";
            string model = "iPhone";
            string color = "White";
            string material = "Glass";

            var product = new TestProductEntity(1000, brand, model, color, material);
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());
            var stock = new ProductStock(product, warehouse, 100);

            
            var retrievedStock = warehouse.GetStockByQualifiers(brand, model, color, material);

            
            Assert.IsNotNull(retrievedStock);
            Assert.AreEqual(stock, retrievedStock);
            Assert.AreEqual(100, retrievedStock.Quantity);
        }

        [Test]
        public void GetStockByQualifiers_ShouldReturnNull_WhenQualifiersDoNotMatch()
        {
            var product = new TestProductEntity(1000, "BrandA", "ModelA", "ColorA", "MaterialA");
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());
            new ProductStock(product, warehouse, 10);

            
            var result = warehouse.GetStockByQualifiers("BrandA", "ModelA", "WRONG_COLOR", "MaterialA");
            
            Assert.IsNull(result);
        }
        
        
        // Error Handling
        
        
        [Test]
        public void Constructor_ShouldThrowException_WhenNegativeQuantity()
        {
            var product = new TestProductEntity(100, "B", "M", "C", "M");
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());

            Assert.Throws<ArgumentException>(() =>
            {
                new ProductStock(product, warehouse, -5);
            });
        }

        [Test]
        public void AddStock_ShouldThrowException_WhenDuplicateQualifiersAddedToSameWarehouse()
        {
            
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());
            
            // Product 1
            var product1 = new TestProductEntity(500, "Dell", "XPS", "Silver", "Metal");
            new ProductStock(product1, warehouse, 10);

            // Product 2 identical Qualifiers
            var product2 = new TestProductEntity(600, "Dell", "XPS", "Silver", "Metal");
            
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                new ProductStock(product2, warehouse, 5);
            });

            StringAssert.Contains("already exists", ex.Message);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenReferencesAreNull()
        {
            var product = new TestProductEntity(100, "B", "M", "C", "M");
            var warehouse = new WarehouseEntity(CreateTestAddress(), GetDefaultStorageRooms());

            // Null Product
            Assert.Throws<ArgumentNullException>(() => new ProductStock(null, warehouse, 10));

            // Null Warehouse
            Assert.Throws<ArgumentNullException>(() => new ProductStock(product, null, 10));
        }
    }
}