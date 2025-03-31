using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using WumpWump.Net.Entities;
using WumpWump.Net.Rest;
using WumpWump.Net.Rest.RateLimits;

namespace WumpWump.Net.Examples.HelloWorld
{
    public sealed class Program
    {
        public static async Task<int> Main(string[] args)
        {
            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (discordToken is null)
            {
                Console.WriteLine("Please set the DISCORD_TOKEN environment variable.");
                return 1;
            }

            DiscordRestClient client = new(NullLogger<DiscordRestClient>.Instance, new DiscordRateLimiter(), new HttpClient(), discordToken);
            DiscordApiResponse<DiscordUser> user = await client.GetCurrentUserAsync(default);

            Debugger.Break();
            return 0;
        }
    }
}
