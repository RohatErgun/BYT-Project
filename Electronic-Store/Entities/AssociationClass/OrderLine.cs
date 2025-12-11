using System;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.AssociationClass
{
    public class OrderLine
    {
        private OrderEntity _order;
        private ProductEntity _product;
        private int _quantity;

        public OrderLine(OrderEntity order, ProductEntity product, int quantity)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

            _order = order;
            _product = product;
            _quantity = quantity;

            _order.AddOrderLine(this);
            _product.AddOrderLine(this);
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0) throw new ArgumentException("Quantity must be greater than zero");
                
                _quantity = value; 
            }
        }

        public OrderEntity Order => _order;
        public ProductEntity Product => _product;

        // specific method to calculate subtotal for this line
        public decimal GetSubTotal()
        {
            return _product.Price * _quantity;
        }
    }
}