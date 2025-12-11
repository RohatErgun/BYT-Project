using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.AssociationClass;

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
        
        public enum PaymentMethodType
        {
            BLIK,
            DebitCard,
            PayPal
        }

        private PaymentMethodType _paymentMethod;
        public PaymentMethodType PaymentMethod
        {
            get => _paymentMethod;
            set => _paymentMethod = value;
        }
        
        private decimal _finalPrice;
        public decimal FinalPrice => _finalPrice;
        
        // association order to product
        private List<OrderLine> _orderLines = new List<OrderLine>();
        public ReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();
        
        public void AddOrderLine(OrderLine line)
        {
            if (line == null) throw new ArgumentNullException(nameof(line));
            
            if (!_orderLines.Contains(line))
            {
                _orderLines.Add(line);
                
                _finalPrice += line.GetSubTotal();
            }
        }
        
        // association order to customer Review
        private List<ReviewEntity> _reviews = new List<ReviewEntity>(); 
        public ReadOnlyCollection<ReviewEntity> Reviews => _reviews.AsReadOnly();
        
        public void AddReview(ReviewEntity review)
        {
            if (review != null && !_reviews.Contains(review))
            {
                _reviews.Add(review);
            }
        }

        public OrderEntity(int id, DateTime date, OrderStatus status, PaymentMethodType paymentMethod)
        {
            Id = id;
            Date = date;
            Status = status;
            PaymentMethod = paymentMethod;

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