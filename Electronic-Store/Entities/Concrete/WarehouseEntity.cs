using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.AssociationClass;
using Electronic_Store.Entities.ComplexAttributes;

namespace Electronic_Store.Entities.Concrete;

[Serializable]
public class WarehouseEntity : BaseEntity
{
    private AddressAttribute _address = null!;
    
    private List<ProductStock> _inventory = new List<ProductStock>();
    public List<ProductStock> Inventory => _inventory;

    public AddressAttribute Address
    {
        get => _address;
        set
        {
            if (value == null)
                throw new ArgumentException("Address cannot be null");
            _address = value;
        }
    }

    public WarehouseEntity(AddressAttribute address)
    {
        Address = address;
    }

    
    public void AddStock(ProductStock stock)
    {
        if (stock == null) return;
            
        var existing = GetStockByQualifiers(
            stock.Product.Brand, stock.Product.Model, 
            stock.Product.Color, stock.Product.Material);

        if (existing != null && existing != stock)
            throw new InvalidOperationException("Product with these qualifiers already exists in this warehouse.");

        if (!_inventory.Contains(stock))
        {
            _inventory.Add(stock);
        }
    }
    
    public ProductStock? GetStockByQualifiers(string brand, string model, string color, string material)
    {
        return _inventory.FirstOrDefault(s => 
            s.Product.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
            s.Product.Model.Equals(model, StringComparison.OrdinalIgnoreCase) &&
            s.Product.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
            s.Product.Material.Equals(material, StringComparison.OrdinalIgnoreCase)
        );
    }
}
