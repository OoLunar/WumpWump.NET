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
using System.Text.Json.Serialization;

namespace WumpWump.Net.Json.Attributes
{
    /// <summary>
    /// Specifies the condition under which any property of this type should be ignored if using the <see cref="ReflectJsonConverter{T}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public sealed class JsonTypeIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Gets the condition under which to ignore properties.
        /// </summary>
        public JsonIgnoreCondition Condition { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonTypeIgnoreAttribute"/> class.
        /// </summary>
        /// <param name="condition">The condition under which to ignore properties.</param>
        public JsonTypeIgnoreAttribute(JsonIgnoreCondition condition) => Condition = condition;
    }
}
