using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    public class PromotionEntity : BaseEntity
    {
        private static List<PromotionEntity> _promotionsExtent = new List<PromotionEntity>();
        public static IReadOnlyList<PromotionEntity> PromotionsExtent => _promotionsExtent.AsReadOnly();

        public static readonly decimal MinPercentage = 5m;
        public static readonly decimal MaxPercentage = 50m;

        private decimal _discountPercentage;
        private string _type;
        private string _description;

        // Promotion - Product (0..*)
        private HashSet<ProductEntity> _products = new();
        public IReadOnlyCollection<ProductEntity> Products => _products;

        public PromotionEntity(decimal discountPercentage, string type, string description)
        {
            DiscountPercentage = discountPercentage;
            Type = type;
            Description = description;

            _promotionsExtent.Add(this);
        }

        
        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (value < MinPercentage || value > MaxPercentage)
                    throw new ArgumentException($"Discount must be between {MinPercentage}% and {MaxPercentage}%.");
                _discountPercentage = value;
            }
        }

        public string Type
        {
            get => _type;
            set => _type = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Type cannot be empty.")
                : value;
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentException("Description cannot be empty.")
                : value;
        }
        //methods
        public void AddProduct(ProductEntity product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (_products.Contains(product))
                return; // duplicate 

            _products.Add(product);

            if (product.Promotion != this)
                product.AssignPromotion(this);
        }

        public void RemoveProduct(ProductEntity product)
        {
            if (product == null)
                return;

            if (!_products.Contains(product))
                return;

            _products.Remove(product);

            if (product.Promotion == this)
                product.RemovePromotion();
        }
    }
}