using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{

    public class ChargesEntity : Accessory
    {
        private double _powerVolt;

        public double PowerVolt
        {
            get => _powerVolt;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Volt cannot be less or equal to zero");
                }

                _powerVolt = value;
            }
        }
        
        public ChargesEntity(
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // Charges params
            double powerVolt)
            : base(price, brand, model, color, material, "Charger")
        {
            PowerVolt = powerVolt;
        }
    }
}