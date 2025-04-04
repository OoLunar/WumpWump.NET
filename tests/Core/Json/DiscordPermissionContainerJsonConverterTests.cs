using System.Numerics;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpWump.Net.Entities;
using WumpWump.Net.Json;

namespace WumpWump.Net.Tests.Core.Json
{
    [TestClass]
    public class DiscordPermissionContainerJsonConverterTests
    {
        private readonly JsonSerializerOptions _options;
        private readonly DiscordPermissionContainerJsonConverter _converter;

        public DiscordPermissionContainerJsonConverterTests()
        {
            _converter = new DiscordPermissionContainerJsonConverter();
            _options = new JsonSerializerOptions();
            _options.Converters.Add(_converter);
        }

        [TestMethod]
        public void ReadEmptyStringReturnsNone()
        {
            // Arrange
            string json = "\"\"";

            // Act
            DiscordPermissionContainer result = JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options);

            // Assert
            Assert.AreEqual(DiscordPermissionContainer.None, result);
        }

        [TestMethod]
        public void ReadValidUlongValueParsesCorrectly()
        {
            // Arrange
            string json = "\"5\""; // 101 in binary

            // Act
            DiscordPermissionContainer result = JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options);

            // Assert
            Assert.IsTrue(result.HasFlag(0));
            Assert.IsFalse(result.HasFlag(1));
            Assert.IsTrue(result.HasFlag(2));
            Assert.IsFalse(result.HasFlag(3));
        }

        [TestMethod]
        public void ReadNumberTooLargeThrowsJsonException()
        {
            // Arrange
            string json = $"\"{new string('9', 100)}\"";

            // Act & Assert
            Assert.ThrowsExactly<JsonException>(() => JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options));
        }

        [TestMethod]
        public void ReadInvalidNumberFormatThrowsJsonException()
        {
            // Arrange
            string json = "\"not_a_number\"";

            // Act & Assert
            Assert.ThrowsExactly<JsonException>(() => JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options));
        }

        [TestMethod]
        public void ReadNonStringTokenThrowsJsonException()
        {
            // Arrange
            string json = "123";

            // Act & Assert
            Assert.ThrowsExactly<JsonException>(() => JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options));
        }

        [TestMethod]
        public void WriteContainerValueWritesCorrectString()
        {
            // Arrange
            DiscordPermissionContainer container = DiscordPermissionContainer.None;
            container.SetFlag(0, true);
            container.SetFlag(2, true); // Should result in value "5" (101 in binary)

            // Act
            string result = JsonSerializer.Serialize(container, _options);

            // Assert
            Assert.AreEqual("\"5\"", result);
        }

        [TestMethod]
        public void ReadMaximumAllowedBitsParsesCorrectly()
        {
            // Arrange
            BigInteger maxBitValue = BigInteger.Pow(2, DiscordPermissionContainer.MAXIMUM_BIT_COUNT - 1);
            string json = $"\"{maxBitValue}\"";

            // Act
            DiscordPermissionContainer result = JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options);

            // Assert
            Assert.IsTrue(result.HasFlag(DiscordPermissionContainer.MAXIMUM_BIT_COUNT - 1));
        }

        [TestMethod]
        public void ReadOneBitOverMaximumThrowsJsonException()
        {
            // Arrange
            BigInteger tooLargeValue = BigInteger.Pow(2, DiscordPermissionContainer.MAXIMUM_BIT_COUNT);
            string json = $"\"{tooLargeValue}\"";

            // Act & Assert
            Assert.ThrowsExactly<JsonException>(() => JsonSerializer.Deserialize<DiscordPermissionContainer>(json, _options));
        }
    }
}