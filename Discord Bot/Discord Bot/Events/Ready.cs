namespace Discord_Bot.Events
{
    using System.Threading.Tasks;
    using Discord;

    public class Ready
    {
        public Ready()
        {
            Program.Client.Ready += Client_Ready;
        }

        private static async Task Client_Ready()
        {
            await Program.Client.SetGameAsync($"{Program.Client.Guilds.Count} {(Program.Client.Guilds.Count == 1 ? "Server" : "Servers")}", null, ActivityType.Watching);
        }
    }
}
