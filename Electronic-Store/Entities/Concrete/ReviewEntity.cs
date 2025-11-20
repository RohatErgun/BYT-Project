using System.Xml;
using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete;
[Serializable]
public class ReviewEntity : BaseEntity
{
    private int _rating;
    private string _text;
    private DateTime _date;

    public int Rating
    {
        get => _rating;

        set
        {
            if (value < 0 || value > 5)
            {
                throw new ArgumentException("Rating must be between 0 and 5.");
            }
            _rating = value;
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Text cannot be empty.");
            }
            _text = value;
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            if (value == DateTime.MinValue)
            {
                throw new ArgumentException("Date cannot be empty.");
            }

            if (value > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be greater than today.");
            }
            _date = value;
        }
    }
    
    private static List<ReviewEntity> _extent = new List<ReviewEntity>();

    private static void AddReview(ReviewEntity review)
    {
        if (review == null)
        {
            throw new ArgumentException("Review cannot be null.");
        }
        _extent.Add(review);
    }

    public static List<ReviewEntity> GetReviews()
    {
        return new List<ReviewEntity>(_extent);
    }

    public ReviewEntity(int rating, string text, DateTime date)
    {
        Rating = rating;
        Text = text;
        Date = date;
        
        AddReview(this);
    }

    public static void Save(string path = "reviews.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ReviewEntity>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, _extent);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving reviews: {e.Message}");
        }
    }

    public static bool Load(string path = "reviews.xml")
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
        XmlSerializer serializer = new XmlSerializer(typeof(List<ReviewEntity>));
        using (XmlReader reader = new XmlTextReader(file))
        {
            try
            {
                _extent = (List<ReviewEntity>)serializer.Deserialize(reader);
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