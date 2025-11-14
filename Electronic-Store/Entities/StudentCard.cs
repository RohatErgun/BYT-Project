using System.Xml;
using System.Xml.Serialization;

namespace Electronic_Store.Entities;

[Serializable]
public class StudentCard
{
    private string _cardNumber;
    private DateTime _expiryDate;

    public string CardNumber
    {
        get => _cardNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Card number cannot be empty");
            }

            _cardNumber = value;
        }
    }

    public DateTime ExpiryDate
    {
        get => _expiryDate;
        set
        {
            if (value < DateTime.Today)
            {
                throw new ArgumentException("Student card expiry date cannot be less than today");
            }

            _expiryDate = value;
        }
    }

    private static List<StudentCard> _extent = new List<StudentCard>();

    private static void AddStudentCard(StudentCard studentCard)
    {
        if (studentCard == null)
        {
            throw new ArgumentException("Student card cannot be null");
        }

        _extent.Add(studentCard);
    }

    public static List<StudentCard> GetStudentCards()
    {
        return new List<StudentCard>(_extent);
    }

    public StudentCard(string cardNumber, DateTime expiryDate)
    {
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;

        AddStudentCard(this);
    }

    public static void Save(string path = "studentCard.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<StudentCard>));
            using StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, _extent);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving student card: {e.Message}");
        }
    }

    public static bool Load(string path = "studentCard.xml")
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

        XmlSerializer serializer = new XmlSerializer(typeof(List<StudentCard>));
        using XmlReader reader = XmlReader.Create(file);
        try
        {
            _extent = (List<StudentCard>)serializer.Deserialize(reader);
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
        return true;
    }

}