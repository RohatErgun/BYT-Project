using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.AssociationClass;
using Electronic_Store.Entities.ComplexAttributes;

namespace Electronic_Store.Entities.Concrete;

[Serializable]
public class WarehouseEntity : BaseEntity
{
    private AddressAttribute _address = null!;
    
    private List<ProductStock> _inventory = new();
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
    
    // Qualified - Ass on; Product Values : brand, model, color, material
    // 
    private Dictionary<string, ProductStock> _qualifiedInventory= new Dictionary<string, ProductStock>();

    // Keys
    private string BuildQualifier(ProductEntity product)
    {
        return $"{product.Brand.ToLowerInvariant()}|" +
               $"{product.Model.ToLowerInvariant()}|" +
               $"{product.Color.ToLowerInvariant()}|" +
               $"{product.Material.ToLowerInvariant()}";
    }
    
    private string BuildQualifier(string brand, string model, string color, string material)
    {
        return $"{brand.ToLowerInvariant()}|" +
               $"{model.ToLowerInvariant()}|" +
               $"{color.ToLowerInvariant()}|" +
               $"{material.ToLowerInvariant()}";
    }
    
    public void AddStock(ProductStock stock)
    {
        if (stock == null) return;

        string key = BuildQualifier(stock.Product);

        if (_qualifiedInventory.TryGetValue(key, out var existing))
        {
            if (existing != stock)
            {
                throw new InvalidOperationException("Product with same brand,model,color and material already exists");
            }
        }
        else
        {
            _qualifiedInventory[key] = stock;
            _inventory.Add(stock);
        }
    }

    public ProductStock? GetStockByQualifiers(string brand, string model, string color, string material)
    {
        string key = BuildQualifier(brand, model, color, material);

        return _qualifiedInventory.TryGetValue(key, out var stock)
            ? stock
            : null;
        
    }
    
}
