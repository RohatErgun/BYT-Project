using System.Xml;
using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.ComplexAttributes;

namespace Electronic_Store.Entities.Concrete;

[Serializable]
public class WarehouseEntity : BaseEntity
{
    //Attributes
    private AddressAttribute _address = null!;

    //Validation checks
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

    //Class extent below
    private static List<WarehouseEntity> _extent = new List<WarehouseEntity>();

    private static void AddWarehouse(WarehouseEntity warehouse)
    {
        if (warehouse == null)
        {
            throw new ArgumentException("Warehouse cannot be null");
        }
        _extent.Add(warehouse);
    }

    public static List<WarehouseEntity> GetWarehouses()
    {
        return new List<WarehouseEntity>(_extent);
    }

    //Constructor
    public WarehouseEntity(AddressAttribute address)
    {
        Address = address;
        AddWarehouse(this);
    }

    //Save
    public static void Save(string path = "warehouses.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<WarehouseEntity>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, _extent);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving warehouse: {e.Message}");
        }
    }

    //Load
    public static bool Load(string path = "warehouses.xml")
    {
        StreamReader file;
        try
        {
            file = File.OpenText(path);
        }
        catch (FileNotFoundException)
        {
            _extent.Clear();
            return false;
        }

        XmlSerializer serializer = new XmlSerializer(typeof(List<WarehouseEntity>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _extent = (List<WarehouseEntity>)serializer.Deserialize(reader);
            }
            catch
            {
                _extent.Clear();
                return false;
            }
        }
        return true;
    }
}
