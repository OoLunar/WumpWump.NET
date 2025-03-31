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

namespace WumpWump.Net.Json.Reflect
{
    /// <summary>
    /// Internal flags used to support ignoring properties.
    /// </summary>
    [Flags]
    internal enum JsonFieldState
    {
        /// <summary>
        /// No relevant state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The property value is <see langword="null"/>.
        /// </summary>
        Null = 1,

        /// <summary>
        /// The property value is equal to the <see langword="default"/> of its type.
        /// </summary>
        Default = 2
    }
}
