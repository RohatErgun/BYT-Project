namespace Electronic_Store.Entities.Abstract
{
    [Flags]
    public enum AccessoryRole
    {
        None    = 0,
        Case    = 1,
        Charger = 2,
        Cable   = 4
    }

    public class CaseInfo
    {
      public string CaseModel { get; set; }

      public CaseInfo(string caseModel)
      {
          CaseModel = caseModel ?? throw new ArgumentNullException(nameof(caseModel));
      }
    }

    public class ChargerInfo
    { 
        public int PowerVolt { get; set; }

        public ChargerInfo(int powerVolt)
        {
            PowerVolt = powerVolt;
        }
    }

    public class CableInfo
    {
        public decimal Length {get; set;}
        public String Type {get;set;}

        public CableInfo(decimal length, string type)
        {
            Length = length;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
    public class Accessory : ProductEntity
    {
        public AccessoryRole Roles { get; private set; }

        private readonly List<ProductEntity> _compatibleModels = new();
        public IReadOnlyList<ProductEntity> CompatibleModels => _compatibleModels.AsReadOnly();

        public CaseInfo? Case { get; set; }
        public ChargerInfo? Charger { get; set; }
        public CableInfo? Cable { get; set; }
        
        public Accessory(
            decimal price,
            string brand,
            string model,
            string color,
            string material,
            AccessoryRole roles
        ) : base(price, brand, model, color, material)
        {
            if (roles == AccessoryRole.None)
                throw new ArgumentException("Accessory must have at least one role.");

            Roles = roles;
        }

        public bool IsCase => Roles.HasFlag(AccessoryRole.Case);
        public bool IsCharger => Roles.HasFlag(AccessoryRole.Charger);
        public bool IsCable => Roles.HasFlag(AccessoryRole.Cable);

        public void ConfigureCase(string caseModel)
        {
            EnsureRole(AccessoryRole.Case);
            Case = new CaseInfo(caseModel);
        }

        public void ConfigureCharger(int powerVolt)
        {
            EnsureRole(AccessoryRole.Charger);
            Charger = new ChargerInfo(powerVolt);
        }

        public void ConfigureCable(decimal length, string type)
        {
            EnsureRole(AccessoryRole.Cable);
            Cable = new CableInfo(length, type);
        }

        public void AddCompatibleModel(ProductEntity product)
        {
            if (product != null && !_compatibleModels.Contains(product))
                _compatibleModels.Add(product);
        }

        private void EnsureRole(AccessoryRole role)
        {
            if (!Roles.HasFlag(role))
                throw new InvalidOperationException($"Accessory does not support role: {role}");
        }
        }
    }