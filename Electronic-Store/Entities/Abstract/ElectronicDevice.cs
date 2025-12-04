using System;

namespace Electronic_Store.Entities.Abstract
{
    [Serializable]
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
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // ElectronicDevice params
            int batteryLifeHours, int warrantyMonths) 
            : base(price, brand, model, color, material) // Pass up to Parent
        {
            BatteryLifeHours = batteryLifeHours;
            WarrantyMonths = warrantyMonths;
        }
    }
}