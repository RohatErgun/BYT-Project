using System.Collections.ObjectModel;
using Electronic_Store.Entities.AssociationClass;

namespace Electronic_Store.Entities.Concrete
{
    [Serializable]
    public class OrderEntity
    {
        
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
        }
        
        
        public CustomerEntity? Customer { get; set; }

        public void AssignCustomer(CustomerEntity customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));

            if (Customer == customer) return;
            
            Customer = customer;
            
            if(!customer.Orders.Contains(this)) customer.AddOrder(this);
        }

        public void RemoveCustomer()
        {
            if (Customer == null) return;
            
            var oldCustomer = Customer;
            Customer = null;
            
            if(oldCustomer.Orders.Contains(this)) oldCustomer.RemoveOrder(this);
        }
    }
}