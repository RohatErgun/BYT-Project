using System;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.AssociationClass
{
    public class ReviewEntity : BaseEntity
    {
        
        private CustomerEntity _customer;
        private OrderEntity _order;

        private int _rating;
        private string _text;
        private DateTime _date;

        public ReviewEntity(CustomerEntity customer, OrderEntity order, int rating, string text, DateTime date)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            if (order == null) throw new ArgumentNullException(nameof(order));

            _customer = customer;
            _order = order;
            
            Rating = rating;
            Text = text;
            Date = date;

            _customer.AddReview(this);
            _order.AddReview(this);
        }

        public CustomerEntity Customer => _customer;
        public OrderEntity Order => _order;

        public int Rating
        {
            get => _rating;
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentException("Rating must be between 0 and 5.");
                _rating = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Text cannot be empty.");
                _text = value;
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (value == DateTime.MinValue)
                    throw new ArgumentException("Date cannot be empty.");
                if (value > DateTime.Now)
                    throw new ArgumentException("Date cannot be greater than today.");
                _date = value;
            }
        }
    }
}