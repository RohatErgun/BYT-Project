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
    
    private List<StorageRoomEntity> _storageRooms = new List<StorageRoomEntity>();
    public IReadOnlyList<StorageRoomEntity> StorageRooms => _storageRooms.AsReadOnly();

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

    public WarehouseEntity(AddressAttribute address, List<(string name, double size)> initialRoomsData)
    {
        Address = address;
        
        // Validate Multiplicity Constraint | cannot be less than 3 rooms
        if (initialRoomsData == null || initialRoomsData.Count < 3)
        {
            throw new ArgumentException("A Warehouse must consist of at least 3 Storage Rooms.");
        }
        
        foreach (var roomData in initialRoomsData)
        {
            new StorageRoomEntity(this, roomData.name, roomData.size);
        }
    }
    
    public void AddStorageRoom(StorageRoomEntity room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        
        if (!_storageRooms.Contains(room) && room.Warehouse == this)
        {
            _storageRooms.Add(room);
        }
    }

    public void RemoveStorageRoom(StorageRoomEntity room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));

        if (_storageRooms.Contains(room))
        {
           
            if (_storageRooms.Count <= 3)
            {
                throw new InvalidOperationException("Cannot remove Storage Room. A Warehouse must have at least 3 rooms.");
            }

            _storageRooms.Remove(room);
            
        }
    }
    
    private Dictionary<string, ProductStock> _qualifiedInventory= new Dictionary<string, ProductStock>();
    
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
