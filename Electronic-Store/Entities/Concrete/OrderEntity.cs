using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class OrderEntity
    {
        private static List<OrderEntity> _orders = new List<OrderEntity>();

        public static ReadOnlyCollection<OrderEntity> GetExtent()
        {
            return _orders.AsReadOnly();
        }

        private static void AddToExtent(OrderEntity orderEntity)
        {
            if (orderEntity == null)
                throw new ArgumentException("Order cannot be null");

            _orders.Add(orderEntity);
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

        private List<ProductEntity> _products = new List<ProductEntity>();

        public ReadOnlyCollection<ProductEntity> Products
        {
            get => _products.AsReadOnly();
        }

        public void AddProduct(ProductEntity p)
        {
            if (p == null)
                throw new ArgumentException("Product cannot be null");

            _products.Add(p);

            _finalPrice += (double)p.Price;
        }

        public OrderEntity(int id, DateTime date, OrderStatus status)
        {
            Id = id;
            Date = date;
            Status = status;

            AddToExtent(this);
        }

        private OrderEntity() { }

        public static void Save(string path = "orders.xml")
        {
            try
            {
                using (StreamWriter file = File.CreateText(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<OrderEntity>));
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
                    XmlSerializer serializer = new XmlSerializer(typeof(List<OrderEntity>));
                    using (XmlTextReader reader = new XmlTextReader(file))
                    {
                        _orders = (List<OrderEntity>)serializer.Deserialize(reader);
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