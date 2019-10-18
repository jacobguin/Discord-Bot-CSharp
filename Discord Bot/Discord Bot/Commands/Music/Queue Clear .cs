using Discord.Commands;
using Discord_Bot.Code_Support.Music;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{

    public class queue : ModuleBase<SocketCommandContext>
    {
        [Command("Queue clear"), Summary("Clears the Queue for music"), Alias("Q C", "Q Clear", "Queue C")]
        public async Task Clear(params string[] args)
        {
            if (args.Length == 0)
            {
                Queue Q = new Queue(Context);
                Q.Clear();
                await Context.Channel.SendMessageAsync("I have cleared the queue.");
            }
            else if (args.Length == 1)
            {
                await Context.Channel.SendMessageAsync("You gave me bad arguments for the queue command.");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You gave me bad arguments for the queue command.");
            }
        }
    }
}
