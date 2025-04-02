// SPDX-License-Identifier: MIT
// This code was taken from OoLunar/DSharpPlus, from the v5-old branch.
// The original author was https://github.com/DPlayer234 from the below commits:
// https://github.com/OoLunar/DSharpPlus/tree/55d13db7dd77f1176b123fc628a15695f73c1277
// https://github.com/OoLunar/DSharpPlus/tree/e5de37aca972852c898daf7ea5d7b959ce5f2085
// https://github.com/OoLunar/DSharpPlus/tree/43e374fe0aa8cd9ddc15ad853621bf9f99997872
// https://github.com/OoLunar/DSharpPlus/tree/ac0c9e3d79eb08a0516b1c181aa5b73fe74d6563
// Full credit goes to them for this file.
// OoLunar has made slight modifications for formatting and to fit the project structure.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using WumpWump.Net.Json.Attributes;
using WumpWump.Net.Json.Reflect;

namespace WumpWump.Net.Json
{
    /// <summary>
    /// A custom <see cref="JsonConverter{T}"/> for any class. Currently respects <see cref="JsonTypeIgnoreAttribute"/>.
    /// </summary>
    /// <typeparam name="T"> The type to be serialized. </typeparam>
    public sealed class ReflectJsonConverter<T> : JsonConverter<T> where T : class
    {
        private static readonly JsonPropertyInfo[]? _properties = BuildInfo().ToArray();
        private static readonly Func<T>? _ctor = JsonReflectionHelper.CompileConstructor<T>();

        /// <inheritdoc/>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(reader.TokenType != JsonTokenType.Null);
            Debug.Assert(_ctor != null);
            Debug.Assert(_properties != null);

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                ThrowInvalidJson();
            }

            T result = _ctor();
            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                Debug.Assert(reader.TokenType == JsonTokenType.PropertyName);

                foreach (JsonPropertyInfo info in _properties)
                {
                    if (reader.ValueTextEquals(info.Name))
                    {
                        info.Read(ref reader, result, options);
                        break;
                    }
                }
            }

            return result;

            static void ThrowInvalidJson()
            {
                throw new JsonException("Expected start of object.");
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Debug.Assert(value != null);
            Debug.Assert(_properties != null);

            writer.WriteStartObject();

            foreach (JsonPropertyInfo info in _properties)
            {
                switch (info.IgnoreCondition ?? options.DefaultIgnoreCondition)
                {
                    case JsonIgnoreCondition.Never:
                        WriteProperty(writer, value, options, info);
                        break;
                    case JsonIgnoreCondition.Always:
                        continue;
                    case JsonIgnoreCondition.WhenWritingDefault:
                        if (!info.GetFieldState(value).HasFlag(JsonFieldState.Default))
                        {
                            WriteProperty(writer, value, options, info);
                        }
                        break;
                    case JsonIgnoreCondition.WhenWritingNull:
                        if (!info.GetFieldState(value).HasFlag(JsonFieldState.Null))
                        {
                            WriteProperty(writer, value, options, info);
                        }
                        break;
                    default:
                        ThrowInvalid();
                        break;
                }
            }

            writer.WriteEndObject();

            static void WriteProperty(Utf8JsonWriter writer, T value, JsonSerializerOptions options, JsonPropertyInfo info)
            {
                writer.WritePropertyName(info.Name);
                info.Write(writer, value, options);
            }

            static void ThrowInvalid()
            {
                throw new InvalidOperationException("Encountered invalid JsonIgnoreCondition.");
            }
        }

        /// <summary>
        /// Builds the property infos used for the <see cref="Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
        /// and <see cref="Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>.
        /// </summary>
        private static IEnumerable<JsonPropertyInfo> BuildInfo()
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // If it can't written or read, skip it
                if (prop.SetMethod == null || prop.GetMethod == null)
                {
                    continue;
                }

                // Only (de)serialize public properties or ones that are explicitly included
                if (!prop.GetMethod.IsPublic)
                {
                    if (prop.IsDefined(typeof(JsonIncludeAttribute)))
                    {
                        throw new InvalidOperationException("The JsonInclude attribute is not allowed on non-public properties.");
                    }

                    continue;
                }

                // Respect the name override
                string name = prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? prop.Name;

                // Consider the ignore condition either on the property or the type
                JsonIgnoreCondition? condition = prop.GetCustomAttribute<JsonIgnoreAttribute>()?.Condition
                    ?? prop.PropertyType.GetCustomAttribute<JsonTypeIgnoreAttribute>()?.Condition;

                yield return new JsonPropertyInfo
                {
                    Write = JsonReflectionHelper.CompileWrite<T>(prop),
                    Read = JsonReflectionHelper.CompileRead<T>(prop),
                    GetFieldState = JsonReflectionHelper.CompileGetFieldState<T>(prop),
                    Name = name,
                    IgnoreCondition = condition
                };
            }
        }

        /// <summary>
        /// Information used to (de)serialize properties.
        /// </summary>
        internal sealed record JsonPropertyInfo
        {
#nullable disable
            public WriteProp<T> Write { get; init; }
            public ReadProp<T> Read { get; init; }
            public Func<T, JsonFieldState> GetFieldState { get; init; }
            public string Name { get; init; }
            public JsonIgnoreCondition? IgnoreCondition { get; init; }
#nullable restore
        }
    }
}
