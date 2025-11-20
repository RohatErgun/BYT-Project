using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{


    [Serializable]
    public class SmartPhoneEntity : ElectronicDevice
    {
        private string _screentype;
        private int _storage;

        public string Screentype
        {
            get => _screentype;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Screen Type cannot be empty");
                }

                _screentype = value;
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
        
    }
}