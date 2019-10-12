using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{
    
    public class Queue_Clear : ModuleBase<SocketCommandContext>
    {
        [Command("Queue"), Summary("Clears the Queue"), Alias("Q Clear", "Q C")]
        public async Task queue()
        {

        }
    }
}
