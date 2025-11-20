using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{

    public class ChargesEntity : Accessory
    {
        private double _powervolt;

        public double PowerVolt
        {
            get => _powervolt;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Volt cannot be less or equal to zero");
                }

                _powervolt = value;
            }
        }
    }
}