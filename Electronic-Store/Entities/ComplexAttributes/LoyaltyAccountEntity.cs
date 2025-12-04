using Electronic_Store.Entities.Abstract;

namespace Electronic_Store.Entities.ComplexAttributes;

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

    public LoyaltyAccountEntity(int balance, Tier tier, DateTime joinDate)
    {
        Balance = balance;
        Tier = tier;
        JoinDate = joinDate;
    }

    public LoyaltyAccountEntity()
    {
    }
}