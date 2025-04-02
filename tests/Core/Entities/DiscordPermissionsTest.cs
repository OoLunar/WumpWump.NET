using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Tests.Core.Entities
{
    [TestClass]
    public class DiscordPermissionsTests
    {
        [TestMethod]
        public void DefaultConstructorInitializesToZero()
        {
            DiscordPermissions perms = new();

            for (int i = 0; i < DiscordPermissions.MAXIMUM_BYTE_COUNT; i++)
            {
                Assert.AreEqual(0, perms[i], $"Byte at index {i} should be 0");
            }
        }

        [TestMethod]
        public void HasFlagReturnsCorrectResults()
        {
            DiscordPermissions admin = DiscordPermissions.Administrator;
            DiscordPermissions manageChannels = DiscordPermissions.ManageChannels;
            DiscordPermissions combined = admin | manageChannels;

            Assert.IsTrue(combined.HasFlag(admin));
            Assert.IsTrue(combined.HasFlag(manageChannels));
            Assert.IsFalse(admin.HasFlag(manageChannels));
            Assert.IsFalse(DiscordPermissions.None.HasFlag(admin));
        }

        [TestMethod]
        public void BitwiseOperatorsWorkCorrectly()
        {
            DiscordPermissions admin = DiscordPermissions.Administrator;
            DiscordPermissions manageChannels = DiscordPermissions.ManageChannels;

            // OR operator
            DiscordPermissions combined = admin | manageChannels;
            Assert.IsTrue(combined.HasFlag(admin));
            Assert.IsTrue(combined.HasFlag(manageChannels));

            // AND operator
            DiscordPermissions andResult = combined & admin;
            Assert.AreEqual(admin, andResult);

            // NOT operator
            DiscordPermissions notAdmin = ~admin;
            Assert.IsFalse(notAdmin.HasFlag(admin));
        }

        [TestMethod]
        public void SetFlagModifiesCorrectBit()
        {
            DiscordPermissions perms = new();

            // Set Administrator flag
            perms.SetFlag(DiscordPermissions.Administrator, true);
            Assert.IsTrue(perms.HasFlag(DiscordPermissions.Administrator));

            // Unset Administrator flag
            perms.SetFlag(DiscordPermissions.Administrator, false);
            Assert.IsFalse(perms.HasFlag(DiscordPermissions.Administrator));
        }

        [TestMethod]
        public void EqualityWorksCorrectly()
        {
            DiscordPermissions admin1 = DiscordPermissions.Administrator;
            DiscordPermissions admin2 = DiscordPermissions.Administrator;
            DiscordPermissions manageChannels = DiscordPermissions.ManageChannels;

            Assert.AreEqual(admin1, admin2);
            Assert.AreNotEqual(admin1, manageChannels);
            Assert.IsTrue(admin1 == admin2);
            Assert.IsTrue(admin1 != manageChannels);
        }

        [TestMethod]
        public void AllHasAllBitsSet()
        {
            DiscordPermissions all = DiscordPermissions.All;

            for (int i = 0; i < DiscordPermissions.MAXIMUM_BYTE_COUNT; i++)
            {
                Assert.AreEqual(0xFF, all[i], $"Byte at index {i} should be 0xFF");
            }
        }

        [TestMethod]
        public void CreateWithBitSetsCorrectBit()
        {
            // Test various bit positions
            for (int bit = 0; bit < DiscordPermissions.MAXIMUM_BIT_COUNT; bit += 13) // Test every 13th bit
            {
                DiscordPermissions perms = DiscordPermissions.Create(bit);
                int byteIndex = bit / 8;
                int bitInByte = bit % 8;
                byte expected = (byte)(1 << bitInByte);

                Assert.AreEqual(expected, perms[byteIndex], $"Bit {bit} should be set");

                // Verify other bytes are 0
                for (int i = 0; i < 16; i++)
                {
                    if (i != byteIndex)
                    {
                        Assert.AreEqual(0, perms[i], $"Byte at index {i} should be 0 for bit {bit}");
                    }
                }
            }
        }
    }
}
