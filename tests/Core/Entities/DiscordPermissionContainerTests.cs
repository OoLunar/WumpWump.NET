using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpWump.Net.Entities;

namespace WumpWump.Net.Tests.Core.Entities
{
    [TestClass]
    public class DiscordPermissionContainerTests
    {
        [TestMethod]
        public void DefaultContainerHasNoPermissions()
        {
            DiscordPermissionContainer container = new();
            Assert.IsFalse(container.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsFalse(container.HasPermission(DiscordPermission.KickMembers));
            Assert.IsFalse(container.HasPermission(DiscordPermission.BanMembers));
            Assert.IsFalse(container.HasPermission(DiscordPermission.Administrator));
        }

        [TestMethod]
        public void SinglePermissionContainerHasCorrectPermission()
        {
            DiscordPermissionContainer container = new(DiscordPermission.CreateInstantInvite);
            Assert.IsTrue(container.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsFalse(container.HasPermission(DiscordPermission.KickMembers));
        }

        [TestMethod]
        public void AdministratorPermissionGrantsAllPermissions()
        {
            DiscordPermissionContainer container = new(DiscordPermission.Administrator);
            Assert.IsTrue(container.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(container.HasPermission(DiscordPermission.KickMembers));
            Assert.IsTrue(container.HasPermission(DiscordPermission.BanMembers));
            Assert.IsTrue(container.HasPermission(DiscordPermission.Administrator));
        }

        [TestMethod]
        public void OrOperatorCombinesPermissions()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.KickMembers);
            DiscordPermissionContainer combined = container1 | container2;

            Assert.IsTrue(combined.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(combined.HasPermission(DiscordPermission.KickMembers));
            Assert.IsFalse(combined.HasPermission(DiscordPermission.BanMembers));
        }

        [TestMethod]
        public void AndOperatorFiltersPermissions()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new DiscordPermissionContainer(DiscordPermission.CreateInstantInvite) | new DiscordPermissionContainer(DiscordPermission.KickMembers);
            DiscordPermissionContainer filtered = container1 & container2;

            Assert.IsTrue(filtered.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsFalse(filtered.HasPermission(DiscordPermission.KickMembers));
        }

        [TestMethod]
        public void XorOperatorTogglesPermissions()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.KickMembers);
            DiscordPermissionContainer toggled = container1 ^ container2;

            Assert.IsTrue(toggled.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(toggled.HasPermission(DiscordPermission.KickMembers));

            toggled ^= container1;
            Assert.IsFalse(toggled.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(toggled.HasPermission(DiscordPermission.KickMembers));
        }

        [TestMethod]
        public void NotOperatorInvertsPermissions()
        {
            DiscordPermissionContainer container = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer inverted = ~container;

            // Should not have the original permission
            Assert.IsFalse(inverted.HasFlag(DiscordPermission.CreateInstantInvite));

            // Should have some other permissions (but not necessarily all)
            Assert.IsTrue(inverted != DiscordPermissionContainer.None);
        }

        [TestMethod]
        public void HasFlagMatchesHasPermissionExceptForAdministrator()
        {
            DiscordPermissionContainer container = new(DiscordPermission.CreateInstantInvite);
            Assert.IsTrue(container.HasFlag(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(container.HasPermission(DiscordPermission.CreateInstantInvite));

            // Administrator grants all permissions through HasPermission but not necessarily through HasFlag
            DiscordPermissionContainer adminContainer = new(DiscordPermission.Administrator);
            Assert.IsTrue(adminContainer.HasPermission(DiscordPermission.CreateInstantInvite));
        }

        [TestMethod]
        public void AsSpanReturnsCorrectBytes()
        {
            DiscordPermissionContainer container = new DiscordPermissionContainer(DiscordPermission.CreateInstantInvite) | new DiscordPermissionContainer(DiscordPermission.KickMembers);
            Span<byte> span = container.AsSpan();

            // CreateInstantInvite is bit 0 (byte 0, bit 0)
            // KickMembers is bit 1 (byte 0, bit 1)
            Assert.AreEqual(3, span[0]);  // 00000011 in binary
            for (int i = 1; i < DiscordPermissionContainer.MAXIMUM_BYTE_COUNT; i++)
            {
                Assert.AreEqual(0, span[i], $"Byte {i} should be 0");
            }
        }

        [TestMethod]
        public void EqualityOperatorsWorkCorrectly()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container3 = new(DiscordPermission.KickMembers);

            Assert.IsTrue(container1 == container2);
            Assert.IsFalse(container1 == container3);
            Assert.IsTrue(container1 != container3);
        }

        [TestMethod]
        public void GetHashCodeReturnsSameValueForEqualContainers()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container3 = new(DiscordPermission.KickMembers);

            Assert.AreEqual(container1.GetHashCode(), container2.GetHashCode());
            Assert.AreNotEqual(container1.GetHashCode(), container3.GetHashCode());
        }

        [TestMethod]
        public void EdgeCasePermissionAtFirstByteBoundary()
        {
            // Test permission at bit 7 (last bit of first byte)
            DiscordPermission permission = (DiscordPermission)7;
            DiscordPermissionContainer container = new(permission);

            Assert.IsTrue(container.HasFlag(permission));
            Assert.AreEqual(128, container.AsSpan()[0]);  // 10000000 in binary
        }

        [TestMethod]
        public void EdgeCasePermissionAtSecondByteStart()
        {
            DiscordPermission permission = (DiscordPermission)8;
            DiscordPermissionContainer container = new(permission);

            Assert.IsTrue(container.HasFlag(permission));

            // Check both possible byte orders
            Assert.IsTrue(container.AsSpan()[1] == 1 || container.AsSpan()[6] == 1);
        }

        [TestMethod]
        public void EdgeCasePermissionAtLastBit()
        {
            // Test permission at bit 63 (last supported bit)
            DiscordPermission permission = (DiscordPermission)DiscordPermissionContainer.MAXIMUM_BIT_COUNT - 1;
            DiscordPermissionContainer container = new(permission);

            Assert.IsTrue(container.HasFlag(permission));
            Assert.AreEqual(128, container.AsSpan()[^1]);
        }

        [TestMethod]
        public void ConstructorThrowsForPermissionBeyondMaxBitCount()
        {
            DiscordPermission permission = (DiscordPermission)DiscordPermissionContainer.MAXIMUM_BIT_COUNT;
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => _ = new DiscordPermissionContainer(permission));
        }

        [TestMethod]
        public void ToStringReturnsCorrectHexRepresentation()
        {
            DiscordPermissionContainer container = new(DiscordPermission.CreateInstantInvite);
            Assert.AreEqual("1".PadLeft(DiscordPermissionContainer.MAXIMUM_BYTE_COUNT * 2, '0'), container.ToHexString());  // 00000001 reversed becomes 0100 in hex
        }

        [TestMethod]
        public void StaticAllHasAllPermissions()
        {
            Assert.IsTrue(DiscordPermissionContainer.All.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsTrue(DiscordPermissionContainer.All.HasPermission(DiscordPermission.KickMembers));
            Assert.IsTrue(DiscordPermissionContainer.All.HasPermission(DiscordPermission.Administrator));
        }

        [TestMethod]
        public void StaticNoneHasNoPermissions()
        {
            Assert.IsFalse(DiscordPermissionContainer.None.HasPermission(DiscordPermission.CreateInstantInvite));
            Assert.IsFalse(DiscordPermissionContainer.None.HasPermission(DiscordPermission.KickMembers));
            Assert.IsFalse(DiscordPermissionContainer.None.HasPermission(DiscordPermission.Administrator));
        }

        [TestMethod]
        public void EqualsReturnsCorrectValue()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.CreateInstantInvite);
            object objContainer = container2;
            object nonContainer = new();

            Assert.IsTrue(container1.Equals(objContainer));
            Assert.IsFalse(container1.Equals(nonContainer));
            Assert.IsFalse(container1.Equals(null));
        }

        [TestMethod]
        public void DifferentPermissionsProduceDifferentHashCodes()
        {
            DiscordPermissionContainer container1 = new(DiscordPermission.CreateInstantInvite);
            DiscordPermissionContainer container2 = new(DiscordPermission.KickMembers);

            Assert.AreNotEqual(container1.GetHashCode(), container2.GetHashCode());
        }
    }
}