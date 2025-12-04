using System;
using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class HeadphonesEntity : ElectronicDevice
    {
        private bool _isWireless;
        private string _type;

        public bool IsWireless
        {
            get => _isWireless;
            set => _isWireless = value;
        }

        public string Type
        {
            get => _type;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Type cannot be empty");
                _type = value;
            }
        }

        // Constructor
        public HeadphonesEntity(
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // ElectronicDevice params
            int batteryLifeHours, int warrantyMonths,
            // Headphones params
            bool isWireless, string type)
            : base(price, brand, model, color, material, batteryLifeHours, warrantyMonths)
        {
            IsWireless = isWireless;
            Type = type;
        }
    }
}