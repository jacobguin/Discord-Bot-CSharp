namespace Discord_Bot.Commands
{
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;

    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("Help")]
        [Summary("Shows a list of commands and gives a summary of them.")]
        public async Task HelpCmd(params string[] command)
        {
            try
            {
                string prefix = Utils.GetPrefix(Context);
                if (command.Length == 0)
                {
                    EmbedBuilder embed = new EmbedBuilder
                    {
                        Title = "Jacob Bot Commands",
                    };

                    string dec = "```markdown\n";

                    foreach (CommandInfo info in Program.Commands.Commands)
                    {
                        dec += $"- {prefix}{info.Name}\n";
                    }

                    embed.WithDescription($"{dec}```");
                    embed.WithFooter("These are all the commands for the bot");

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else if (command.Length == 1)
                {
                    foreach (CommandInfo info1 in Program.Commands.Commands)
                    {
                        if (command[0].ToLower() == info1.Name.ToLower())
                        {
                            await Context.Channel.SendMessageAsync(info1.Summary);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "Help", ex);
            }
        }
    }
}
