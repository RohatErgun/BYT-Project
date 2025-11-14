using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public abstract class ProductEntity : BaseEntity
    {
        // Class extent
        private static List<ProductEntity> _productsExtent = new List<ProductEntity>();
        public static IReadOnlyList<ProductEntity> ProductsExtent => _productsExtent.AsReadOnly();

     
        // Mandatory attributes
        private decimal _price;
        private string _brand;
        private string _model;
        private string _color;
        private string _material;

        // Constructor
        protected ProductEntity(decimal price, string brand, string model, string color, string material)
        {
            // Validate mandatory attributes
            if (price < 0) throw new ArgumentException("Price cannot be negative.");
            if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Brand cannot be empty.");
            if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model cannot be empty.");
            if (string.IsNullOrWhiteSpace(color)) throw new ArgumentException("Color cannot be empty.");
            if (string.IsNullOrWhiteSpace(material)) throw new ArgumentException("Material cannot be empty.");

            
            _price = price;
            _brand = brand;
            _model = model;
            _color = color;
            _material = material;

            // Add to class extent
            AddProduct(this);
        }

        // Properties
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public string Brand
        {
            get => _brand;
            set => _brand = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Brand cannot be empty.") : value;
        }

        public string Model
        {
            get => _model;
            set => _model = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Model cannot be empty.") : value;
        }

        public string Color
        {
            get => _color;
            set => _color = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Color cannot be empty.") : value;
        }

        public string Material
        {
            get => _material;
            set => _material = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Material cannot be empty.") : value;
        }

        // Class extent management
        private static void AddProduct(ProductEntity productEntity)
        {
            if (productEntity == null) throw new ArgumentException("Product cannot be null.");
            _productsExtent.Add(productEntity);
        }

        // Extent persistence
        public static void SaveExtent(string path = "products.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(path);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ProductEntity>), new Type[] { typeof(ProductEntity) });
                xmlSerializer.Serialize(file, _productsExtent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving product extent: " + ex.Message);
            }
        }

        public static bool LoadExtent(string path = "products.xml")
        {
            try
            {
                if (!File.Exists(path))
                {
                    _productsExtent.Clear();
                    return false;
                }

                using StreamReader file = File.OpenText(path);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ProductEntity>), new Type[] { typeof(ProductEntity) });
                _productsExtent = (List<ProductEntity>)xmlSerializer.Deserialize(file) ?? new List<ProductEntity>();
            }
            catch
            {
                _productsExtent.Clear();
                return false;
            }

            return true;
        }
    }
}
