namespace Discord_Bot.Commands.Music
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord_Bot.Code_Support.Music;

    public class Queue_Clear : ModuleBase<SocketCommandContext>
    {
        [Command("Queue")]
        [Summary("Queue for music")]
        [Alias("Q")]
        public async Task Queue()
        {
            try
            {
                Code_Support.Music.Queue q = new Code_Support.Music.Queue(Context);
                string desc = $"```markdown{Environment.NewLine}";
                if (q.Items == null)
                {
                    await Context.Channel.SendMessageAsync("there is nothing in the queue");
                    return;
                }

                for (int i = 0; i < q.Items.Length - 1; i++)
                {
                    WebClient web = new WebClient();
                    string text = web.DownloadString($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={q.Items[i].YtVideo.ID}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}");
                    string title = Json.Parse(text, "items[0].snippet.title");
                    desc += $"{i + 1}. [{title}]{Environment.NewLine}";
                }

                EmbedFooterBuilder footer = new EmbedFooterBuilder()
                    .WithText($"Requested by {Context.Message.Author.Username}#{Context.Message.Author.Discriminator}")
                    .WithIconUrl(Context.Message.Author.GetAvatarUrl());
                EmbedBuilder embed = new EmbedBuilder()
                    .WithDescription($"{desc}```")
                    .WithTitle("Current Queue")
                    .WithFooter(footer);
                await Context.Channel.SendMessageAsync(null, false, embed.Build());
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "Queue", ex);
            }
        }
    }
}
