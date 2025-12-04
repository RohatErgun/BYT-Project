using System;
using System.Collections.Generic;

namespace Electronic_Store.Entities.Abstract
{
    [Serializable]
    public abstract class Accessory : ProductEntity
    {
        // Example: Type of accessory (Cable, Case, Charger)
        public string Category { get; set; } 

        // List of products this accessory works with
        private List<ProductEntity> _compatibleModels = new List<ProductEntity>();
        public IReadOnlyList<ProductEntity> CompatibleModels => _compatibleModels.AsReadOnly();

        protected Accessory(
            decimal price, string brand, string model, string color, string material,
            string category)
            : base(price, brand, model, color, material)
        {
            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category cannot be empty");
            Category = category;
        }

        public void AddCompatibleModel(ProductEntity product)
        {
            if(product != null && !_compatibleModels.Contains(product))
            {
                _compatibleModels.Add(product);
            }
        }
    }
}