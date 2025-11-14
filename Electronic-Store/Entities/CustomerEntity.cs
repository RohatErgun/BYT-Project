using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities;

[Serializable]
public class CustomerEntity : BaseEntity
{   
    //Attributes
    private string _name;
    private string _surname;
    private AddressAttribute _address;
    private List<string> _email = new List<string>();
    private string _phoneNumber;
    private DateTime _birthDate;
    private int _age;
    private bool? _studentCard;
    
    //Validation checks
    public string Name
    {
        get => _name;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            _name = value;
        }
    }

    public string Surname
    {
        get => _surname;
        set
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Surname cannot be empty");
            }
            _surname = value;
        }
    }

    public List<string> Email
    {
        get => new List<string>(_email);
    }

    public void SetEmail(IEnumerable<string> emails)
    {
        if (emails == null)
        {
            _email.Clear();
            return;
        }
        
        List<string> validatedEmails = new List<string>();
        foreach (var email in emails)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }

            if (!email.Contains("@"))
            {
                throw new ArgumentException($"Email {email} is not valid");
            }
            validatedEmails.Add(email);
        }
        _email = validatedEmails;
    }

    public void AddEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        if (!email.Contains("@"))
        {
            throw new ArgumentException($"Email {email} is not valid");
        }
        
        _email.Add(email);
    }

    public void RemoveEmail(string email)
    {
        _email.Remove(email);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("PhoneNumber cannot be empty");
            }
            _phoneNumber = value;
        }
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            if (value == DateTime.MinValue)
            {
                throw new ArgumentException("Birthdate cannot be empty");
            }

            if (value > DateTime.Now)
            {
                throw new ArgumentException("Birthdate cannot be greater than today");
            }
            _birthDate = value;
        }
    }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - _birthDate.Year;
            if (_birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

    public bool? StudentCard
    {
        get => _studentCard;
        set
        {
            if (value.HasValue && value.Value == true && Age < 17)
            {
                throw new ArgumentException("Student card can't be less than 17");
            }
            _studentCard = value;
        }
    }
    
    //Class extent below
    private static List<CustomerEntity> _extent = new List<CustomerEntity>();

    private static void AddCustomer(CustomerEntity customer)
    {
        if (customer == null)
        {
            throw new ArgumentException("Customer cannot be null");
        }
        _extent.Add(customer);
    }

    public static List<CustomerEntity> GetCustomer()
    {
        return new List<CustomerEntity>(_extent);
    }

    public CustomerEntity(string name, string surname, AddressAttribute address, string phoneNumber, DateTime birthDate, int age, bool? studentCard)
    {
        _name = name;
        _surname = surname;
        _address = address;
        _phoneNumber = phoneNumber;
        _birthDate = birthDate;
        _age = age;
        _studentCard = studentCard;
        
        AddCustomer(this);
    }

    public static void Save(string path = "customers.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerEntity>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, _extent);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving customer: {e.Message}");
        }
    }

    public static bool Load(string path = "customers.xml")
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
        XmlSerializer serializer = new XmlSerializer(typeof(List<CustomerEntity>));
        using (XmlTextReader reader = new XmlTextReader(file))
        {
            try
            {
                _extent = (List<CustomerEntity>)serializer.Deserialize(reader);
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