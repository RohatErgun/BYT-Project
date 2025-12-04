namespace Electronic_Store.Entities.ComplexAttributes;

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

    public StudentCard(string cardNumber, DateTime expiryDate)
    {
        CardNumber = cardNumber;
        ExpiryDate = expiryDate;
    }
}