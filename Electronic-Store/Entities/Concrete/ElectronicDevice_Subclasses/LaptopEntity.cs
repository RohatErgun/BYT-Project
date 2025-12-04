using Electronic_Store.Entities.Abstract;
using Microsoft.VisualBasic.CompilerServices;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class LaptopEntity : ElectronicDevice

    {
        // Attributes 
        private string _screenType;
        private int _ram;
        private int _storage;

        // Properties 
        public string ScreenType
        {
            get => _screenType;
            
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Screen Type cannot be empty");    
                }

                _screenType = value;
            }
        }

        public int Ram
        {
            get => _ram;
            set
            {
                if (value < 4 || value > 128)
                {
                    throw new ArgumentException("Ram is between 4 to 128");
                }

                _ram = value;
            }
        }
        public int Storage
        {
            get => _storage;
            set
            {
                if (value <= 0) throw new ArgumentException("Storage must be positive");
                _storage = value;
            }
        }
        
        // Constructor 
        public LaptopEntity(
            // Product params
            decimal price, string brand, string model, string color, string material,
            // Device params
            int batteryLifeHours, int warrantyMonths,
            // Laptop params
            string screenType, int ram, int storage)
            : base(price, brand, model, color, material, batteryLifeHours, warrantyMonths) 
        {
            ScreenType = screenType;
            Ram = ram;
            Storage = storage;
        }
    }
}