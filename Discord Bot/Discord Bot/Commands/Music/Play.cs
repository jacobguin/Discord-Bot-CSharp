namespace Discord_Bot.Commands.Music
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Audio;
    using Discord.Commands;
    using Discord.Rest;
    using Discord.WebSocket;
    using Discord_Bot.Code_Support.Music;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Play : ModuleBase<SocketCommandContext>
    {
        private readonly string[] emojiArr = { "1⃣", "2⃣", "3⃣", "4⃣", "5⃣" };
        private JArray items = null;
        private IVoiceChannel channel = null;

        [Command("Play", RunMode = RunMode.Async)]
        [Summary("Music")]
        public async Task PlayCmd(params string[] search)
        {
            try
            {
                if (search.Length == 0)
                {
                    if (string.IsNullOrEmpty(Database.Read<string>("music", "server_id", Context.Guild.Id.ToString(), "queue")))
                    {
                        await Context.Channel.SendMessageAsync("Nothing's in the queue. Please provide a song.");
                    }
                    else
                    {
                        channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
                        if (channel == null)
                        {
                            await Context.Channel.SendMessageAsync("You must be in a voice channel.");
                        }
                        else if (!Database.Read<bool>("music", "server_id", Context.Guild.Id.ToString(), "playing"))
                        {
                            Code_Support.Music.Queue q = new Code_Support.Music.Queue(Context);
                            await Playing.StartPlaying(await channel.ConnectAsync(), Context, q);
                        }
                        else
                        {
                            await Context.Channel.SendMessageAsync("The bot is already playing something.");
                        }
                    }
                }
                else
                {
                    IEnumerable<IMessage> msgs = await Context.Channel.GetMessagesAsync().FlattenAsync();
                    if (msgs.Where(m => m.Author.Id == Hidden_Info.Tokens.Id && m.Content.StartsWith("**Song Results:**")).Count() > 0)
                    {
                        await Context.Channel.SendMessageAsync("There is already a message to select a song. Please select one in order to add more songs.");
                        return;
                    }

                    channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
                    if (channel == null)
                    {
                        await Context.Channel.SendMessageAsync("You must be in a voice channel.");
                    }
                    else
                    {
                        string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={Uri.EscapeUriString(string.Join(" ", search))}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}";

                        WebClient webClient = new WebClient();
                        string value = webClient.DownloadString(url);

                        items = JsonConvert.DeserializeObject<dynamic>(value).items;

                        if (items.Count == 0)
                        {
                            await Context.Channel.SendMessageAsync("No search results found.");
                        }
                        else
                        {
                            int index = 0;
                            dynamic[] titles = items.Select(i =>
                            {
                                index += 1;
                                return index + ": " + i.Value<dynamic>().snippet.title;
                            }).ToArray();
                            string msg = $"**Song Results:**\n```\n{string.Join("\n", titles)}\n```**This message will expire in 10 seconds!**";
                            RestUserMessage m = await Context.Channel.SendMessageAsync(msg);
                            int results = items.Count;
                            int current = 1;
                            Context.Client.ReactionAdded += Client_ReactionAdded;
                            try
                            {
                                foreach (string em in emojiArr)
                                {
                                    if (current <= results)
                                    {
                                        current++;
                                        Emoji e = new Emoji(em);
                                        await m.AddReactionAsync(e);
                                    }
                                }

                                for (int i = 0; i < 9; i++)
                                {
                                    await Task.Delay(1000);
                                    await m.ModifyAsync(h => h.Content = msg.Replace("**This message will expire in 10 seconds!**", $"**This message will expire in {9 - i} seconds!**"));
                                }

                                await Task.Delay(1000);
                                Context.Client.ReactionAdded -= Client_ReactionAdded;
                                await m.DeleteAsync();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "Play", ex);
            }
        }

        private async Task Client_ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                int num = Array.IndexOf(emojiArr, reaction.Emote.Name) + 1;

                IUserMessage msg = await message.DownloadAsync();
                if (msg == null) return;
                if (reaction.UserId != Hidden_Info.Tokens.Id && msg.Author.Id == Hidden_Info.Tokens.Id && emojiArr.Contains(reaction.Emote.Name) && msg.ToString().StartsWith("**Song Results:**"))
                {
                    if (items.Count < num)
                    {
                        await msg.Channel.SendMessageAsync("No result with that number found.");
                        return;
                    }

                    Context.Client.ReactionAdded -= Client_ReactionAdded;
                    await msg.DeleteAsync();
                    string id = items.Value<dynamic>(num - 1).id.videoId;
                    _ = Start(id);
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "play ReactionAdded", ex);
            }
        }

        private async Task Start(string id)
        {
            try
            {
                Code_Support.Music.Queue q = new Code_Support.Music.Queue(Context);
                q.Add(Code_Support.Music.Queue.Type.Youtube, id);
                if (!Database.Read<bool>("music", "server_id", Context.Guild.Id.ToString(), "playing"))
                {
                    IAudioClient audioClient = await channel.ConnectAsync();
                    _ = Playing.StartPlaying(audioClient, Context, q);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("The bot is already playing something but i added your song to the Queue");
                }
            }
            catch (Exception ex)
            {
                await Utils.ReportError(Context, "play start", ex);
            }
        }
    }
}
