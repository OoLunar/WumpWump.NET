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
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WumpWump.Net.Json
{
    /// <summary>
    /// A <see cref="JsonConverterFactory"/> for instances of <see cref="ReflectJsonConverter{T}"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This converter should only be applied on the <see cref="JsonSerializerOptions"/> level.
    /// To apply the converter on type-level, use <see cref="ReflectJsonConverter{T}"/> directly.
    /// </para>
    /// <para>
    /// This type will automatically cover any type that meets the following criteria if added to <see cref="JsonSerializerOptions"/>:
    /// </para>
    /// <list type="bullet">
    /// <item>Is a class (not a struct or interface)</item>
    /// <item>Is not a type in the <c>System</c> namespace</item>
    /// <item>Has no custom type-level converter</item>
    /// <item>Does not implement <see cref="IEnumerable"/></item>
    /// </list>
    /// </remarks>
    public sealed class ReflectJsonConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
            => !typeToConvert.IsInterface
            && !typeToConvert.IsValueType
            && !typeToConvert.IsDefined(typeof(JsonConverterAttribute))
            && !typeToConvert.Namespace!.StartsWith("System", false, CultureInfo.InvariantCulture)
            && !typeof(IEnumerable).IsAssignableFrom(typeToConvert);

        /// <inheritdoc/>
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) => (JsonConverter)typeof(ReflectJsonConverter<>).MakeGenericType(typeToConvert).GetConstructor(Type.EmptyTypes)!.Invoke(null)!;
    }
}
