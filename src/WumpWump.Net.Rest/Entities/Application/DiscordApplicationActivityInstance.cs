using System.Collections.Generic;

namespace WumpWump.Net.Rest.Entities
{
    public record DiscordApplicationActivityInstance
    {
        /// <summary>
        /// <see cref="DiscordApplication"/> ID
        /// </summary>
        public required DiscordSnowflake Id { get; init; }

        /// <summary>
        /// Activity <a href="https://discord.com/developers/docs/activities/development-guides#activity-instance-management">Instance</a> ID
        /// </summary>
        public required string InstanceId { get; init; }

        /// <summary>
        /// Unique identifier for the launch
        /// </summary>
        public required DiscordSnowflake LaunchId { get; init; }

        /// <summary>
        /// Location the instance is running in
        /// </summary>
        public required DiscordApplicationActivityLocation Location { get; init; }

        /// <summary>
        /// IDs of the Users currently connected to the instance
        /// </summary>
        public required IReadOnlyList<DiscordSnowflake> Users { get; init; }
    }
}
