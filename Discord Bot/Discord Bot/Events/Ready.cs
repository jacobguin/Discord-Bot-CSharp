using Discord;
using System.Threading.Tasks;

namespace Discord_Bot.Events
{
    public static class Ready
    {
        public static void Load()
        {
            Program.Client.Ready += Client_Ready;
        }

        private async static Task Client_Ready()
        {
            await Program.Client.SetGameAsync(Program.Client.Guilds.Count.ToString() + " Servers", null, ActivityType.Watching);
        }
    }
}
