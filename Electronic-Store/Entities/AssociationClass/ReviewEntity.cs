using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.AssociationClass;
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

    public ReviewEntity(int rating, string text, DateTime date)
    {
        Rating = rating;
        Text = text;
        Date = date;
        
    }
    
}