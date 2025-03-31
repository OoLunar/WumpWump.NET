using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WumpWump.Net.Entities;
using WumpWump.Net.Json;

namespace WumpWump.Net.Tests.Core.Json
{
    [TestClass]
    public class JsonOptional
    {
        private record RecordWithOptional(DiscordOptional<int> Value);

        private static readonly JsonSerializerOptions stjOptions = new()
        {
            TypeInfoResolver = DiscordJsonTypeInfoResolver.Default
        };

        [TestMethod]
        public void Deserialize()
        {
            RecordWithOptional? empty = JsonSerializer.Deserialize<RecordWithOptional>("{}", stjOptions);
            Assert.AreEqual(new RecordWithOptional(DiscordOptional<int>.Empty), empty);

            RecordWithOptional? withFive = JsonSerializer.Deserialize<RecordWithOptional>("""{"Value": 5}""", stjOptions);
            Assert.AreEqual(new RecordWithOptional(5), withFive);
        }

        [TestMethod]
        public void Serialize()
        {
            string empty = JsonSerializer.Serialize(new RecordWithOptional(DiscordOptional<int>.Empty), stjOptions);
            Assert.AreEqual("{}", empty);

            string withFive = JsonSerializer.Serialize(new RecordWithOptional(5), stjOptions);
            Assert.AreEqual("""{"Value":5}""", withFive);
        }
    }
}
