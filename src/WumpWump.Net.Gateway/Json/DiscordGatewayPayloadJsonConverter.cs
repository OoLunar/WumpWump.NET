using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using WumpWump.Net.Gateway.Entities;
using WumpWump.Net.Gateway.Events;
using WumpWump.Net.Gateway.Payloads;

namespace WumpWump.Net.Gateway.Json
{
    public class DiscordGatewayPayloadJsonConverter : JsonConverter<IDiscordGatewayPayload>
    {
        protected static readonly ConcurrentDictionary<Type, Func<DiscordGatewayOpCode, string?, ulong?, object, IDiscordGatewayPayload>> _payloadFactory = new();

        public FrozenDictionary<(DiscordGatewayOpCode OpCode, string? EventName), Type> PayloadTypes { get; init; }

        public DiscordGatewayPayloadJsonConverter(DiscordGatewayEventRegistration eventRegistrations)
        {
            ArgumentNullException.ThrowIfNull(eventRegistrations, nameof(eventRegistrations));

            Dictionary<(DiscordGatewayOpCode OpCode, string? EventName), Type> payloadTypes = [];
            foreach (DiscordGatewayEventTicket ticket in eventRegistrations)
            {
                payloadTypes.Add((ticket.OpCode, ticket.EventName), ticket.EventType);
            }

            PayloadTypes = payloadTypes.ToFrozenDictionary();
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(IDiscordGatewayPayload));

        public override void Write(Utf8JsonWriter writer, IDiscordGatewayPayload value, JsonSerializerOptions options)
        {
            // Write the payload as a JSON object
            writer.WriteStartObject();

            // Write the event name
            writer.WritePropertyName("t");
            writer.WriteStringValue(value.EventName);

            // Write the sequence number
            writer.WritePropertyName("s");
            if (value.Sequence.HasValue)
            {
                writer.WriteNumberValue(value.Sequence.Value);
            }
            else
            {
                writer.WriteNullValue();
            }

            // Write the op code
            writer.WritePropertyName("op");
            writer.WriteNumberValue((int)value.OpCode);

            // Write the data
            writer.WritePropertyName("d");
            if (value.Data is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Data, value.Data.GetType(), options);
            }

            // End the JSON object
            writer.WriteEndObject();
        }

        public override IDiscordGatewayPayload? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Failed to read start of DiscordGatewayPayload<T>: Expected {JsonTokenType.StartObject}, found {reader.TokenType}");
            }


            string? eventName = null;
            ulong? sequence = null;
            DiscordGatewayOpCode? opCode = null;
            Type? dataType = null;
            object? data = null;
            ReadProperties(options, ref reader, ref eventName, ref sequence, ref opCode, ref dataType, ref data);

            // Construct the most specific DiscordGatewayPayload<T> type
            dataType ??= typeof(DiscordGatewayNullPayload);
            Func<DiscordGatewayOpCode, string?, ulong?, object, IDiscordGatewayPayload> payloadFactory = GetPayloadFactory(dataType);
            return payloadFactory(opCode.Value, eventName, sequence, data!);
        }

        protected void ReadProperties(JsonSerializerOptions options, ref Utf8JsonReader reader, ref string? eventName, ref ulong? sequence, [NotNull] ref DiscordGatewayOpCode? opCode, ref Type? dataType, ref object? data)
        {
            // Start reading properties, try to optimize for the
            // data property (d) to be the last property we read
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }
                else if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Expected {JsonTokenType.PropertyName}, found {reader.TokenType}");
                }
                else if (reader.ValueSpan.Length == 0)
                {
                    throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Expected property name to be a non-empty string");
                }
                // Micro-optimization here! The if statements are in the
                // same order as the properties that Discord sends to us.
                else if (TryGetPropertyValue("t"u8, GetEventName, ref reader, ref eventName))
                {
                    continue;
                }
                else if (TryGetPropertyValue("s"u8, GetSequence, ref reader, ref sequence))
                {
                    continue;
                }
                else if (TryGetPropertyValue("op"u8, GetOpCode, ref reader, ref opCode))
                {
                    continue;
                }
                else if (!reader.ValueSpan.SequenceEqual("d"u8))
                {
                    // Skip the property if it's not one of the expected ones
                    reader.Skip();
                    continue;
                }

                if (dataType is not null)
                {
                    // This should never happen, however if we want to support
                    // other hosts besides Discord in the future then we should
                    // probably make these safety checks just in case.
                    throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Expected only one data property");
                }

                // Check if we found the op code before the data property
                bool shouldBreak = false;
                if (opCode is null)
                {
                    // If we haven't found the op code yet, we need to
                    // copy the data and then parse it later

                    // Copy the current state of the reader, which should be at the start of the data property
                    Utf8JsonReader copy = reader;

                    // Skip the data property
                    reader.Skip();

                    // Read the rest of the properties
                    ReadProperties(options, ref reader, ref eventName, ref sequence, ref opCode, ref dataType, ref data);

                    // Since the reader is now at the end of the object, we can
                    // replace it with the copy and read just the data property
                    reader = copy;
                    shouldBreak = true;
                }

                // If we have found the op code and the event name,
                // we can deserialize the data property now.
                if (PayloadTypes.TryGetValue((opCode.Value, eventName), out dataType))
                {
                    data = JsonSerializer.Deserialize(ref reader, dataType, options);
                }
                else
                {
                    // Serialize into a JsonElement
                    data = new DiscordGatewayUnknownPayload()
                    {
                        Json = (JsonObject)JsonNode.Parse(ref reader, new JsonNodeOptions())!
                    };

                    dataType = typeof(DiscordGatewayUnknownPayload);
                }

                if (shouldBreak)
                {
                    // Earlier when we copied the reader, it read the other
                    // properties to completion. We don't need to read the
                    // entire object twice, so just break here.
                    break;
                }
            }

            if (!opCode.HasValue)
            {
                throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Expected op code to be present");
            }
        }

        protected delegate T GetValueFromJsonMethod<T>(ref Utf8JsonReader reader);
        protected static bool TryGetPropertyValue<T>(ReadOnlySpan<byte> propertyName, GetValueFromJsonMethod<T> getValueMethod, ref Utf8JsonReader reader, ref T? value)
        {
            if (!reader.ValueSpan.SequenceEqual(propertyName))
            {
                return false;
            }
            else if (value is not null)
            {
                throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Expected only one {Encoding.UTF8.GetString(propertyName)} property");
            }

            value = getValueMethod(ref reader);
            return true;
        }

        protected static string? GetEventName(ref Utf8JsonReader reader) => reader.Read() ? reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read event name: Expected the value to be a string or null")
        } : throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read event name: Not enough data");

        protected static ulong? GetSequence(ref Utf8JsonReader reader) => reader.Read() ? reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetUInt64(),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read sequence number: Expected the value to be a number or null")
        } : throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read sequence number: Not enough data");

        protected static DiscordGatewayOpCode? GetOpCode(ref Utf8JsonReader reader) => reader.Read() ? reader.TokenType switch
        {
            JsonTokenType.Number => (DiscordGatewayOpCode)reader.GetInt32(),
            _ => throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read op code: Expected the value to be an integer")
        } : throw new JsonException($"Failed to read DiscordGatewayPayload<T>: Failed to read op code: Not enough data");

        protected static Func<DiscordGatewayOpCode, string?, ulong?, object, IDiscordGatewayPayload> GetPayloadFactory(Type payloadType)
            => _payloadFactory.GetOrAdd(payloadType, (Type payloadType) =>
            {
                // Create a new factory method for the payload type
                MethodInfo method = typeof(DiscordGatewayPayloadJsonConverter).GetMethod(
                    nameof(PayloadFactory),
                    BindingFlags.NonPublic | BindingFlags.Static,
                    [typeof(DiscordGatewayOpCode), typeof(string), typeof(ulong?), typeof(object)]
                ) ?? throw new UnreachableException($"Failed to find factory method for {payloadType.FullName}");

                MethodInfo genericMethod = method.MakeGenericMethod(payloadType);
                return genericMethod.CreateDelegate<Func<DiscordGatewayOpCode, string?, ulong?, object, IDiscordGatewayPayload>>();
            });

        protected static IDiscordGatewayPayload PayloadFactory<T>(DiscordGatewayOpCode opCode, string? eventName, ulong? sequence, object data) => new DiscordGatewayPayload<T>()
        {
            OpCode = opCode,
            EventName = eventName,
            Sequence = sequence,
            Data = (T)data,
        };
    }
}
