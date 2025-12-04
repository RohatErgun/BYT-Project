using Electronic_Store.Entities.Abstract;
using Electronic_Store.Entities.ComplexAttributes;

namespace Electronic_Store.Entities.Concrete;

public class CustomerEntity : BaseEntity
{   
    //Attributes
    private string _name = null!;
    private string _surname = null!;
    private AddressAttribute _address = null!;
    private List<string> _email = new List<string>();
    private string _phoneNumber = null!;
    private DateTime _birthDate;
    private StudentCard? _studentCard;
    
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

    public List<string> Email => new List<string>(_email);

    public void SetEmail(IEnumerable<string> emails)
    {
        if (emails == null)
        {
            throw new ArgumentException("Customer should have at least one email");
        }
        
        var list = emails.ToList();
        if (list.Count < 1 || list.Count > 3)
        {
            throw new ArgumentException("Customer should have from 1 to 3 emails");
        }

        foreach (var email in list)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be empty");
            }

            if (!email.Contains('@'))
            {
                throw new ArgumentException($"Email {email} is not valid");
            }
        }
        _email = list;
    }

    public void AddEmail(string email)
    {
        if (_email.Count > 3)
        {
            throw new ArgumentException("Customer should have at most 3 emails");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty");
        }

        if (!email.Contains('@'))
        {
            throw new ArgumentException($"Email {email} is not valid");
        }
        _email.Add(email);
    }

    public void RemoveEmail(string email)
    {
        if (_email.Count <= 1)
        {
            throw new ArgumentException("Customer should have at least one email");
        }
        _email.Remove(email);
    }

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

    public StudentCard? StudentCard
    {
        get => _studentCard;
        set
        {
            if (value != null && Age < 17)
            {
                throw new ArgumentException("Customer should be at least 17 years old");
            }
            _studentCard = value;
        }
    }
    
    //Constructor
    public CustomerEntity(string name, string surname, IEnumerable<string> email, AddressAttribute address, string phoneNumber, DateTime birthDate, StudentCard? studentCard = null)
    {
        Name = name;
        Surname = surname;
        Address = address;
        PhoneNumber = phoneNumber;
        BirthDate = birthDate;
        SetEmail(email);
        StudentCard = studentCard;
    }
}