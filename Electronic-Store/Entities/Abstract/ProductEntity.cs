using Electronic_Store.Entities.AssociationClass;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.Abstract
{
    public enum ProductCondition
    {
        New,
        Refurbished
    }

    public class NewProductInfo
    {
        public DateTime ManufacturingDate { get;}
        public TimeSpan WarrantyPeriod { get;}

        public NewProductInfo(DateTime manufacturingDate, TimeSpan warrantyPeriod)
        {
            ManufacturingDate = manufacturingDate;
            WarrantyPeriod = warrantyPeriod;
        }
    }

    public class RefurbishedProductInfo
    {
        public decimal DiscountPercentage { get;}

        public RefurbishedProductInfo()
        {
            DiscountPercentage = 0.10m;
        }
    }
    public abstract class ProductEntity : BaseEntity
    {

        private decimal _price;
        private string _brand;
        private string _model;
        private string _color;
        private string _material;

        public ProductCondition Condition { get;}
        
        public NewProductInfo? NewInfo { get;}
        public RefurbishedProductInfo? RefurbishedInfo { get;}
    
        protected ProductEntity(
            decimal price, 
            string brand, 
            string model,
            string color,
            string material,
            ProductCondition condition,
            NewProductInfo? newInfo =null,
            RefurbishedProductInfo? refurbishedInfo = null)
        {
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
            
            ValidateCondition(condition, newInfo, refurbishedInfo);
            Condition = condition;
            NewInfo = newInfo;
            RefurbishedInfo = refurbishedInfo;
        }

        private static void ValidateCondition(
            ProductCondition condition,
            NewProductInfo? newInfo,
            RefurbishedProductInfo? refurbishedInfo)
        {
            if (condition == ProductCondition.New)
            {
                if (newInfo == null)
                    throw new ArgumentException("New product requires NewProductInfo.");
                if (refurbishedInfo != null)
                    throw new ArgumentException("New product cannot have RefurbishedProductInfo.");
            }
            if (condition == ProductCondition.Refurbished)
            {
                if (refurbishedInfo == null)
                    throw new ArgumentException("Refurbished product requires RefurbishedProductInfo.");
                if (newInfo != null)
                    throw new ArgumentException("Refurbished product cannot have NewProductInfo.");
            }
        }
        public decimal GetEffectivePrice()
        {
            if (Condition == ProductCondition.Refurbished)
                return Price * (1 - RefurbishedInfo!.DiscountPercentage);

            return Price;
        }
        // PRODUCT - WAREHOUSE
        private List<ProductStock> _stocks = new List<ProductStock>();
        public List<ProductStock> Stocks => _stocks;


        // PRODUCT - ORDER
        private List<OrderLine> _orderLines = new List<OrderLine>();
        public IReadOnlyList<OrderLine> OrderLines => _orderLines.AsReadOnly();


        // PRODUCT 0..1 - 0..* PROMOTION
        public PromotionEntity? Promotion { get; private set; }
        
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
            set => _brand = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Brand cannot be empty.")
                : value;
        }

        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Model cannot be empty.")
                : value;
        }

        public string Color
        {
            get => _color;
            set => _color = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Color cannot be empty.")
                : value;
        }

        public string Material
        {
            get => _material;
            set => _material = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Material cannot be empty.")
                : value;
        }


        // PRODUCT - WAREHOUSE
        public void AddStock(ProductStock stock)
        {
            if (stock != null && !_stocks.Contains(stock))
                _stocks.Add(stock);
        }


        // PRODUCT - ORDER
        public void AddOrderLine(OrderLine line)
        {
            if (line != null && !_orderLines.Contains(line))
                _orderLines.Add(line);
        }


        // METHODS FOR PROMOTION
        public void AssignPromotion(PromotionEntity promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            // prevents infinite recursion
            if (Promotion == promotion)
                return;

            var oldPromotion = Promotion;
            Promotion = promotion;

            // Remove from old promotion 
            if (oldPromotion != null && oldPromotion.Products.Contains(this))
                oldPromotion.RemoveProduct(this);

            // Add to new promotion 
            if (!promotion.Products.Contains(this))
                promotion.AddProduct(this);
        }


        public void RemovePromotion()
        {
            var oldPromotion = Promotion;
            Promotion = null;

            if (oldPromotion != null && oldPromotion.Products.Contains(this))
                oldPromotion.RemoveProduct(this);
        }

        public WorkerEntity? AddedBy { get; set; }

        public void AssignWorker(WorkerEntity worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException(nameof(worker));
            }

            if (AddedBy == worker)
            {
                return;
            }
            
            AddedBy = worker;

            if (!worker.Products.Contains(this))
            {
                worker.AddProduct(this);
            }
        }

        public void ReassignWorker()
        {
            if (AddedBy == null)
            {
                return;
            }
            
            var oldWorker = AddedBy;
            AddedBy = null;

            if (oldWorker.Products.Contains(this))
            {
                oldWorker.RemoveProduct(this);
            }
        }
    }
}