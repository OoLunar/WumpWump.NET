namespace WumpWump.Net.Rest.Entities
{
    public enum DiscordApplicationRoleConnectionMetadataType
    {
        /// <summary>
        /// the metadata value (<c>integer</c>) is less than or equal to the guild's configured value (<c>integer</c>)
        /// </summary>
        IntegerLessThanOrEqual = 1,

        /// <summary>
        /// the metadata value (<c>integer</c>) is greater than or equal to the guild's configured value (<c>integer</c>)
        /// </summary>
        IntegerGreaterThanOrEqual = 2,

        /// <summary>
        /// the metadata value (<c>integer</c>) is equal to the guild's configured value (<c>integer</c>)
        /// </summary>
        IntegerEqual = 3,

        /// <summary>
        /// the metadata value (<c>integer</c>) is not equal to the guild's configured value (<c>integer</c>)
        /// </summary>
        IntegerNotEqual = 4,

        /// <summary>
        /// the metadata value (<c>ISO8601 string</c>) is less than or equal to the guild's configured value (<c>integer</c>; <c>days before current date</c>)
        /// </summary>
        DatetimeLessThanOrEqual = 5,

        /// <summary>
        /// the metadata value (<c>ISO8601 string</c>) is greater than or equal to the guild's configured value (<c>integer</c>; <c>days before current date</c>)
        /// </summary>
        DatetimeGreaterThanOrEqual = 6,

        /// <summary>
        /// the metadata value (<c>integer</c>) is equal to the guild's configured value (<c>integer</c>; <c>1</c>)
        /// </summary>
        BooleanEqual = 7,

        /// <summary>
        /// the metadata value (<c>integer</c>) is not equal to the guild's configured value (<c>integer</c>; <c>1</c>)
        /// </summary>
        BooleanNotEqual = 8
    }
}
