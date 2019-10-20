namespace Discord_Bot.Events
{
    using System.Threading.Tasks;
    using Discord;

    public static class Ready
    {
        public static void Load()
        {
            Program.Client.Ready += Client_Ready;
        }

        private static async Task Client_Ready()
        {
            await Program.Client.SetGameAsync(Program.Client.Guilds.Count.ToString() + " Servers", null, ActivityType.Watching);
        }
    }
}
