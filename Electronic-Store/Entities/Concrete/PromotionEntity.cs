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

        public PromotionEntity(decimal discountPercentage, string type, string description)
        {
            DiscountPercentage = discountPercentage;
            Type = type;
            Description = description;

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
    }
}
