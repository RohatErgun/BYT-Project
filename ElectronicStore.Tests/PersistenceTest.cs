using System.Reflection;
using Electronic_Store.Entities.Concrete;

namespace ElectronicStore.Tests
{
    public class LoyaltyAccountEntityTests
    {
        private string GetTempFile() =>
            Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml");

        [SetUp]
        public void Setup()
        {
            typeof(LoyaltyAccountEntity)
                .GetField("_extent", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, new List<LoyaltyAccountEntity>());
        }

        [Test]
        public void Save_ShouldCreateFile()
        {
            string path = GetTempFile();
            new LoyaltyAccountEntity(10, Tier.Basic, DateTime.Today);

            LoyaltyAccountEntity.Save(path);

            Assert.True(File.Exists(path));
        }

        [Test]
        public void Load_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            string path = GetTempFile();

            bool result = LoyaltyAccountEntity.Load(path);

            Assert.False(result);
            Assert.AreEqual(0, LoyaltyAccountEntity.GetLoyaltyAccounts().Count);
        }

        [Test]
        public void SaveLoad_ShouldRestoreExtentCorrectly()
        {
            string path = GetTempFile();

            var acc = new LoyaltyAccountEntity(100, Tier.Premium, DateTime.Today);

            LoyaltyAccountEntity.Save(path);
            LoyaltyAccountEntity.Load(path);

            var extent = LoyaltyAccountEntity.GetLoyaltyAccounts();
            
            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(100, extent[0].Balance);
            Assert.AreEqual(Tier.Premium, extent[0].Tier);
            Assert.AreEqual(DateTime.Today, extent[0].JoinDate.Date);
        }

        [Test]
        public void Load_ShouldClearExtent_OnCorruptedFile()
        {
            string path = GetTempFile();
            File.WriteAllText(path, "NOT_VALID_XML");

            bool result = LoyaltyAccountEntity.Load(path);

            Assert.False(result);
            Assert.AreEqual(0, LoyaltyAccountEntity.GetLoyaltyAccounts().Count);
        }

        [Test]
        public void SaveLoad_ShouldRestoreMultipleObjects()
        {
            string path = GetTempFile();

            new LoyaltyAccountEntity(10, Tier.Basic, DateTime.Today);
            new LoyaltyAccountEntity(20, Tier.Deluxe, DateTime.Today.AddDays(-1));

            LoyaltyAccountEntity.Save(path);
            LoyaltyAccountEntity.Load(path);

            var extent = LoyaltyAccountEntity.GetLoyaltyAccounts();

            Assert.AreEqual(2, extent.Count);
        }

        [Test]
        public void Load_ShouldOverwriteInMemoryExtent()
        {
            string path = GetTempFile();

            new LoyaltyAccountEntity(50, Tier.Vip, DateTime.Today);
            LoyaltyAccountEntity.Save(path);

            new LoyaltyAccountEntity(999, Tier.Basic, DateTime.Today);

            LoyaltyAccountEntity.Load(path);

            var extent = LoyaltyAccountEntity.GetLoyaltyAccounts();

            Assert.AreEqual(1, extent.Count);
            Assert.AreEqual(50, extent[0].Balance);
        }
    }
}