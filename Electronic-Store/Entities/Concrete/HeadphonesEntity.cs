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
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Type cannot be empty");
                }
                _type = value;
            }
            
        }
        
        
    }
}