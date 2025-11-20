using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{

    public class CasesEntity : Accessory
    {
        private string _color;

        public string Color
        {
            get => _color;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Color cannot be empty");    
                }

                _color = value;

            }
            
        }
    }
}