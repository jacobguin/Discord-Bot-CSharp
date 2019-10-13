using Discord.Commands;
using System.Threading.Tasks;

namespace Discord_Bot.Commands.Music
{
    
    public class queue : ModuleBase<SocketCommandContext>
    {
        [Command("Queue cleare"), Summary("Queue commands fors the bot"), Alias("Q")]
        public async Task Queue(params string[] args)
        {
            if (args.Length == 0)
            {
                Code_Support.Music.Queue.Type[] type;
                string[] songs = Code_Support.Music.Queue.List(Context, out type);
                string s = "s";
            }
            else if (args.Length == 1)
            { 
                //if ()
            }
            else
            {
                await Context.Channel.SendMessageAsync("You gave me bad arguments for the que command.");
            }
        }
    }
}
