using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class CablesEntity : Accessory
    {
        private int _lenght;
        private string _type;

        
        public int Lenght
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
    }
}