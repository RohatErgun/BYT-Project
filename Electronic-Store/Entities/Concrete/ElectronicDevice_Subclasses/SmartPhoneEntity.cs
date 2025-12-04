using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{


    [Serializable]
    public class SmartPhoneEntity : ElectronicDevice
    {
        private string _screenType;
        private int _storage;

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

        public int Storage
        {
            get => _storage;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Storage of labtop cannot be empty");
                }

                _storage = value;
            }
        }
        
        // Constructor 
        public SmartPhoneEntity(
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // ElectronicDevice params
            int batteryLifeHours, int warrantyMonths,
            // SmartPhone params
            string screenType, int storage)
            : base(price, brand, model, color, material, batteryLifeHours, warrantyMonths)
        {
            ScreenType = screenType;
            Storage = storage;
        }
    }
}