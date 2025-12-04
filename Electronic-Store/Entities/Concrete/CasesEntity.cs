using System;
using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class CasesEntity : Accessory
    {
        // Attributes
        private string _compatibleDeviceModel;

        public string CompatibleDeviceModel
        {
            get => _compatibleDeviceModel;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Compatible Device Model cannot be empty");
                }
                _compatibleDeviceModel = value;
            }
        }

        // Constructor 
        public CasesEntity(
            // ProductEntity params
            decimal price, string brand, string model, string color, string material,
            // CasesEntity params
            string compatibleDeviceModel)
            : base(price, brand, model, color, material, "Case")
        {
            CompatibleDeviceModel = compatibleDeviceModel;
        }  
    }
}