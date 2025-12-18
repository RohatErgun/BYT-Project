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
    public class Accessory : ProductEntity
    {
        public AccessoryRole Roles { get; private set; }

        private readonly List<ProductEntity> _compatibleModels = new();
        public IReadOnlyList<ProductEntity> CompatibleModels => _compatibleModels.AsReadOnly();

        public string? CaseModel { get; private set; }

        public int? PowerVolt { get; private set; }

        public decimal? CableLength { get; private set; }
        public string? CableType { get; private set; }

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
            CaseModel = caseModel;
        }

        public void ConfigureCharger(int powerVolt)
        {
            EnsureRole(AccessoryRole.Charger);
            PowerVolt = powerVolt;
        }

        public void ConfigureCable(decimal length, string type)
        {
            EnsureRole(AccessoryRole.Cable);
            CableLength = length;
            CableType = type;
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