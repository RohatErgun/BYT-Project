using Electronic_Store.Entities.Association;

namespace Electronic_Store.Entities.Abstract
{
    [Serializable]
    public abstract class ProductEntity : BaseEntity
    {
        // Class extent
        private static List<ProductEntity> _productsExtent = new List<ProductEntity>();
        public static IReadOnlyList<ProductEntity> ProductsExtent => _productsExtent.AsReadOnly();

     
        // Mandatory attributes
        private decimal _price;
        private string _brand;
        private string _model;
        private string _color;
        private string _material;
        
        // Association: List of connections to Warehouses
        private List<ProductStock> _stocks = new List<ProductStock>();
        public List<ProductStock> Stocks => _stocks;
        

        // Constructor
        protected ProductEntity(decimal price, string brand, string model, string color, string material)
        {
            // Validate mandatory attributes
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Brand cannot be empty.");
            if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model cannot be empty.");
            if (string.IsNullOrWhiteSpace(color)) throw new ArgumentException("Color cannot be empty.");
            if (string.IsNullOrWhiteSpace(material)) throw new ArgumentException("Material cannot be empty.");

            
            _price = price;
            _brand = brand;
            _model = model;
            _color = color;
            _material = material;

            // Automatically add to extend when ANY child is created
            _productsExtent.Add(this);
        }

        // Properties
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public string Brand
        {
            get => _brand;
            set => _brand = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Brand cannot be empty.") : value;
        }

        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Model cannot be empty.") : value;
        }

        public string Color
        {
            get => _color;
            set => _color = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Color cannot be empty.") : value;
        }

        public string Material
        {
            get => _material;
            set => _material = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Material cannot be empty.") : value;
        }
        
        // Association Management
        public void AddStock(ProductStock stock)
        {
            if (stock != null && !_stocks.Contains(stock))
            {
                _stocks.Add(stock);
            }
        }

        // Class extent management
        private static void AddProduct(ProductEntity productEntity)
        {
            if (productEntity == null) throw new ArgumentException("Product cannot be null.");
            _productsExtent.Add(productEntity);
        }
    }
}
