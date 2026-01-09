namespace Electronic_Store.Entities.Abstract
{
    [Flags]
    public enum AccessoryRole
    {
        None = 0,
        Case = 1,
        Charger = 2,
        Cable = 4
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
        public decimal Length { get; set; }
        public String Type { get; set; }

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

        public CaseInfo? Case { get;}
        public ChargerInfo? Charger { get;}
        public CableInfo? Cable { get;}

        public bool IsCase => Roles.HasFlag(AccessoryRole.Case);
        public bool IsCharger => Roles.HasFlag(AccessoryRole.Charger);
        public bool IsCable => Roles.HasFlag(AccessoryRole.Cable);

        public Accessory(
            decimal price,
            string brand,
            string model,
            string color,
            string material,
            AccessoryRole roles,
            ProductCondition condition = ProductCondition.New,
            CaseInfo? caseInfo = null,
            ChargerInfo? chargerInfo = null,
            CableInfo? cableInfo = null
        ) : base(
            price,
            brand,
            model,
            color,
            material,
            condition,
            newInfo: condition == ProductCondition.New ? new NewProductInfo(DateTime.UtcNow, TimeSpan.FromDays(365)) : null,
            refurbishedInfo: condition == ProductCondition.Refurbished ? new RefurbishedProductInfo() : null
        )
        {
            if (roles == AccessoryRole.None)
                throw new ArgumentException("Accessory must have at least one role.");

            ValidateRoleData(roles, caseInfo, chargerInfo, cableInfo);

            Roles = roles;
            Case = caseInfo;
            Charger = chargerInfo;
            Cable = cableInfo;
        }

        private static void ValidateRoleData(
            AccessoryRole roles,
            CaseInfo? caseInfo,
            ChargerInfo? chargerInfo,
            CableInfo? cableInfo)
        {
            if (roles.HasFlag(AccessoryRole.Case) && caseInfo == null)
                throw new ArgumentException("Case role requires CaseInfo.");

            if (!roles.HasFlag(AccessoryRole.Case) && caseInfo != null)
                throw new ArgumentException("CaseInfo provided but Case role not specified.");

            if (roles.HasFlag(AccessoryRole.Charger) && chargerInfo == null)
                throw new ArgumentException("Charger role requires ChargerInfo.");

            if (!roles.HasFlag(AccessoryRole.Charger) && chargerInfo != null)
                throw new ArgumentException("ChargerInfo provided but Charger role not specified.");

            if (roles.HasFlag(AccessoryRole.Cable) && cableInfo == null)
                throw new ArgumentException("Cable role requires CableInfo.");

            if (!roles.HasFlag(AccessoryRole.Cable) && cableInfo != null)
                throw new ArgumentException("CableInfo provided but Cable role not specified.");
        }

        public void AddCompatibleModel(ProductEntity product)
        {
            if (product != null && !_compatibleModels.Contains(product))
                _compatibleModels.Add(product);
        }
    }
}