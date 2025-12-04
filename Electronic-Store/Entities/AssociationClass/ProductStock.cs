using System;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.Concrete;

namespace Electronic_Store.Entities.AssociationClass
{
    public class ProductStock
    {
        private ProductEntity _product;
        private WarehouseEntity _warehouse;
        private int _quantity;

        public ProductStock(ProductEntity product, WarehouseEntity warehouse, int quantity)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (warehouse == null) throw new ArgumentNullException(nameof(warehouse));
            if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.");

            _product = product;
            _warehouse = warehouse;
            _quantity = quantity;
            
            //links
            _product.AddStock(this);
            _warehouse.AddStock(this);
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0) throw new ArgumentException("Quantity cannot be negative");
                _quantity = value;
            }
        }

        public ProductEntity Product => _product;
        public WarehouseEntity Warehouse => _warehouse;
    }
}