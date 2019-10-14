using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public static class Playing
    {
        private static IAudioClient c;
        private static IVoiceChannel v;
        private static SocketCommandContext s;

        public static async Task StartPlaying(IAudioClient Client, SocketCommandContext Context, IVoiceChannel Channel)
        {
            Database.Update("Music", "Playing", "Server_ID", Context.Guild.Id.ToString(), true);
            v = Channel;
            s = Context;
            c = Client;
            Program.Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;
            while (true)
            {
                try
                {
                    try
                    {
                        Queue.Queue_Item Content = Queue.FirstInQueue(Context);
                        if (Content.Type == Queue.Type.End)
                        {
                            await Queue.Clear(Context);
                            await Stop(Client, Context);
                            break;
                        }
                        else if (Content.Type == Queue.Type.Youtube)
                        {
                            await Context.Channel.SendMessageAsync("", false, Youtube_video_embed(Content.YtVideo.ID.Replace("https://www.youtube.com/watch?v=", "")));
                            await Backend.SendUrlAsync(Client, Content.YtVideo.ID);
                        }
                        else if (Content.Type == Queue.Type.Playlist)
                        {
                            //not here yet
                        }
                    }
                    catch (Exception ex)
                    {
                        await Utils.RepotError(Context, "Play", ex);
                    }
                }
                finally
                {
                    await Queue.RemoveFirst(Context);
                }
            }
            await Stop(Client, Context);
        }

        private async static Task Client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            int count = PeopleInCall(s, v);
            if (count == 0)
            {
                await Stop(c, s);
            }
        }

        private static async Task Stop(IAudioClient Client, SocketCommandContext Context)
        {
            Database.Update("Music", "Playing", "Server_ID", Context.Guild.Id.ToString(), false);
            await Client.StopAsync();
        }

        private static Embed Youtube_video_embed(string ID)
        {
            EmbedBuilder e = new EmbedBuilder();
            WebClient web = new WebClient();
            string text = web.DownloadString("https://www.googleapis.com/youtube/v3/videos?part=snippet&id=" + ID + "&key=" + Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube));
            e.Title = "Now Playing: " + '"' + Json.Parse(text, "items[0].snippet.title") + '"' + " : by: " + Json.Parse(text, "items[0].snippet.channelTitle");
            e.WithThumbnailUrl(Json.Parse(text, "items[0].snippet.thumbnails.standard.url"));
            e.WithColor(255, 0, 0);
            e.WithFooter($"https://www.youtube.com/watch?v={ID}");
            return e.Build();
        }

        private static int PeopleInCall(SocketCommandContext Context, IVoiceChannel Channel)
        {
            return Context.Client.GetGuild(Context.Guild.Id).GetVoiceChannel(Channel.Id).Users.Count - 1;
        }
    }
}
