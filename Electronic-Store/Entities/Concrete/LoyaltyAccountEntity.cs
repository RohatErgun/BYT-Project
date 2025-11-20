using System.Xml;
using System.Xml.Serialization;
using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.Concrete;

[Serializable]
public enum Tier
{
    Basic,
    Premium,
    Deluxe,
    Vip
}
public class LoyaltyAccountEntity : BaseEntity
{
    private int _balance;
    private Tier _tier = Tier.Basic;
    private DateTime _joinDate;

    public int Balance
    {
        get => _balance;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Balance cannot be less than 0");
            }
            _balance = value;
        }
    }

    public Tier Tier
    {
        get => _tier;
        set
        {
            if (Tier == null)
            {
                throw new ArgumentException("Tier cannot be null");
            }
            _tier = value;
        }
    }

    public DateTime JoinDate
    {
        get => _joinDate;
        set
        {
            if (value == DateTime.MinValue)
            {
                throw new ArgumentException("Join date cannot be empty");
            }

            if (value > DateTime.Now)
            {
                throw new ArgumentException("Join date cannot be greater than today");
            }
            _joinDate = value;
        }
    }
    
    private static List<LoyaltyAccountEntity> _extent = new List<LoyaltyAccountEntity>();

    private static void AddLoyaltyAccount(LoyaltyAccountEntity loyaltyAccount)
    {
        if (loyaltyAccount == null)
        {
            throw new ArgumentException("loyaltyAccount cannot be null");
        }
        _extent.Add(loyaltyAccount);
    }

    public static List<LoyaltyAccountEntity> GetLoyaltyAccounts()
    {
        return new List<LoyaltyAccountEntity>(_extent);
    }

    public LoyaltyAccountEntity(int balance, Tier tier, DateTime joinDate)
    {
        Balance = balance;
        Tier = tier;
        JoinDate = joinDate;
        
        AddLoyaltyAccount(this);
    }

    public static void Save(string path = "loyaltyaccount.xml")
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LoyaltyAccountEntity));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, _extent);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving loyalty account: {e.Message}");
        }
    }

    public static bool Load(string path = "loyaltyaccount.xml")
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
        XmlSerializer serializer = new XmlSerializer(typeof(List<LoyaltyAccountEntity>));
        using (XmlReader reader = new XmlTextReader(file))
        {
            try
            {
                _extent = (List<LoyaltyAccountEntity>)serializer.Deserialize(reader);
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