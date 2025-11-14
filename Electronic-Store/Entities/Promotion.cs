using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Electronic_Store.Entities
{
    [Serializable]
    public class Promotion
    {
        // Class extent
        private static List<Promotion> _promotionsExtent = new List<Promotion>();
        public static IReadOnlyList<Promotion> PromotionsExtent => _promotionsExtent.AsReadOnly();

        // Class attributes (static)
        public static readonly decimal MinPercentage = 5m;
        public static readonly decimal MaxPercentage = 50m;

        // Attributes
        private decimal _discountPercentage;
        private string _type;
        private string _description;

        // Constructor
        public Promotion(decimal discountPercentage, string type, string description)
        {
            DiscountPercentage = discountPercentage;
            Type = type;
            Description = description;

            AddPromotion(this);
        }

        // Properties with validation
        public decimal DiscountPercentage
        {
            get => _discountPercentage;
            set
            {
                if (value < MinPercentage || value > MaxPercentage)
                    throw new ArgumentException($"Discount must be between {MinPercentage}% and {MaxPercentage}%.");
                _discountPercentage = value;
            }
        }

        public string Type
        {
            get => _type;
            set => _type = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Type cannot be empty.") : value;
        }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Description cannot be empty.") : value;
        }

        // Class extent management
        private static void AddPromotion(Promotion promotion)
        {
            if (promotion == null) throw new ArgumentException("Promotion cannot be null.");
            _promotionsExtent.Add(promotion);
        }

        // Persistence
        public static void SaveExtent(string path = "promotions.xml")
        {
            try
            {
                using StreamWriter file = File.CreateText(path);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Promotion>));
                xmlSerializer.Serialize(file, _promotionsExtent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving promotion extent: " + ex.Message);
            }
        }

        public static bool LoadExtent(string path = "promotions.xml")
        {
            try
            {
                if (!File.Exists(path))
                {
                    _promotionsExtent.Clear();
                    return false;
                }

                using StreamReader file = File.OpenText(path);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Promotion>));
                _promotionsExtent = (List<Promotion>)xmlSerializer.Deserialize(file) ?? new List<Promotion>();
            }
            catch
            {
                _promotionsExtent.Clear();
                return false;
            }

            return true;
        }
    }
}
