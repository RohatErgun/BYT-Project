using Electronic_Store.Entities.Abstract;
using Microsoft.VisualBasic.CompilerServices;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class LabtopEntity : ElectronicDevice

    {
        private string _screentype;
        private int _ram;
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
        
    }
}