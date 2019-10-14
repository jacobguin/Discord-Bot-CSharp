using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{

    public class queue : ModuleBase<SocketCommandContext>
    {
        [Command("Queue clear"), Summary("Queue commands for the bot"), Alias("Q C", "Q Clear", "Queue C")]
        public async Task Clear(params string[] args)
        {
            if (args.Length == 0)
            {
                await Code_Support.Music.Queue.Clear(Context);
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
