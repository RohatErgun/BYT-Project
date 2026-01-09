namespace Electronic_Store.Entities.Abstract
{
    public abstract class ElectronicDevice : ProductEntity
    {
        // --- Attributes ---
        private int _batteryLifeHours;
        private int _warrantyMonths;

        // --- Properties ---
        public int BatteryLifeHours
        {
            get => _batteryLifeHours;
            set
            {
                if (value < 0) throw new ArgumentException("Battery life cannot be negative.");
                _batteryLifeHours = value;
            }
        }

        public int WarrantyMonths
        {
            get => _warrantyMonths;
            set
            {
                if (value < 0) throw new ArgumentException("Warranty cannot be negative.");
                _warrantyMonths = value;
            }
        }

        //  Constructor 
        protected ElectronicDevice(
            decimal price,
            string brand,
            string model,
            string color,
            string material,
            int batteryLifeHours,
            int warrantyMonths,
            ProductCondition condition = ProductCondition.New
        ) : base(
            price,
            brand,
            model,
            color,
            material,
            condition,
            newInfo: condition == ProductCondition.New ? new NewProductInfo(DateTime.UtcNow, TimeSpan.FromDays(warrantyMonths * 30)) : null,
            refurbishedInfo: condition == ProductCondition.Refurbished ? new RefurbishedProductInfo() : null
        )
        {
            BatteryLifeHours = batteryLifeHours;
            WarrantyMonths = warrantyMonths;
        }
    }
}