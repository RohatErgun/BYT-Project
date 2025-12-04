using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities.ComplexAttributes;

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
    

    public AddressAttribute(string country, string city, string street, int buildingNumber, string zipcode)
    {
        Country = country;
        City = city;
        Street = street;
        BuildingNumber = buildingNumber;
        Zipcode = zipcode;
    }
    
}