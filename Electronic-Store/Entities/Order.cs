using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities
{
    [Serializable]
    public class Order
    {
        private static List<Order> _orders = new List<Order>();

        public static ReadOnlyCollection<Order> GetExtent()
        {
            return _orders.AsReadOnly();
        }

        private static void AddToExtent(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order cannot be null");

            _orders.Add(order);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Id must be greater than 0");
                _id = value;
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                if (value == default)
                    throw new ArgumentException("Date cannot be empty");
                _date = value;
            }
        }

        public enum OrderStatus
        {
            Delivered,
            OnTheWay
        }

        private OrderStatus _status;
        public OrderStatus Status
        {
            get => _status;
            set => _status = value;
        }

        private double _finalPrice;
        public double FinalPrice => _finalPrice;

        private List<Product> _products = new List<Product>();

        public ReadOnlyCollection<Product> Products
        {
            get => _products.AsReadOnly();
        }

        public void AddProduct(Product p)
        {
            if (p == null)
                throw new ArgumentException("Product cannot be null");

            _products.Add(p);

            _finalPrice += (double)p.Price;
        }

        public Order(int id, DateTime date, OrderStatus status)
        {
            Id = id;
            Date = date;
            Status = status;

            AddToExtent(this);
        }

        private Order() { }

        public static void Save(string path = "orders.xml")
        {
            try
            {
                using (StreamWriter file = File.CreateText(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Order>));
                    using (XmlTextWriter writer = new XmlTextWriter(file))
                    {
                        serializer.Serialize(writer, _orders);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving extent: " + e.Message);
            }
        }

        public static bool Load(string path = "orders.xml")
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Order>));
                    using (XmlTextReader reader = new XmlTextReader(file))
                    {
                        _orders = (List<Order>)serializer.Deserialize(reader);
                    }
                }
            }
            catch
            {
                _orders.Clear();
                return false;
            }
            return true;
        }
    }
}