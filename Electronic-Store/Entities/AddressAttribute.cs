using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities;

[Serializable]
public class AddressAttribute
{
    private string _country;
    private string _city;
    private string _street;
    private int _buildingNumber;
    private string _zipcode;

    public string Country
    {
        get => _country;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Country cannot be empty");
            }
            _country = value;
        }
    }
    
    public string City
    {
        get => _city;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("City cannot be empty");
            }
            _city = value;
        }
    }
    
    public string Street
    {
        get => _street;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Street cannot be empty");
            }
            _street = value;
        }
    }

    public int BuildingNumber
    {
        get => _buildingNumber;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Building number cannot be negative");
            }
            _buildingNumber = value;
        }
    }
    
    public string Zipcode
    {
        get => _zipcode;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Zipcode cannot be empty");
            }
            _zipcode = value;
        }
    }
    
    private static List<AddressAttribute> _extent = new List<AddressAttribute>();

    private static void AddAddress(AddressAttribute address)
    {
        if (address == null)
        {
            throw new ArgumentException("Address cannot be null");
        }
        _extent.Add(address);
    }

    public static List<AddressAttribute> GetAddress()
    {
        return new List<AddressAttribute>(_extent);
    }

    public AddressAttribute(string country, string city, string street, int buildingNumber, string zipcode)
    {
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        Zipcode = zipcode;
        
        AddAddress(this);
    }

    public static void Save(string path = "address.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AddressAttribute>));
            using StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, _extent);
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving address: {e.Message}");
        }
    }

    public static bool Load(string path = "address.xml")
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
        XmlSerializer serializer = new XmlSerializer(typeof(List<AddressAttribute>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _extent = (List<AddressAttribute>)serializer.Deserialize(reader);
            }
            catch (InvalidCastException)
            {
                _extent.Clear();
                return false;
            }
            catch (Exception)
            {
                _extent.Clear();
                return false;
            }
        }
        return true;
    }
}