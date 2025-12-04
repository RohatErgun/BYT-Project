using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class CablesEntity : Accessory
    {
        private int _lenght;
        private string _type;

        
        public int Length
        {
            get => _lenght;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Lenght of cable cannot be empty or less then zero");
                }

                _lenght = value;
            }
           
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
        
        public CablesEntity(
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // Cables params
            int length, string type)
            : base(price, brand, model, color, material, "Cable")
        {
            Length = length;
            Type = type;
        }
    }
}