namespace Discord_Bot.Code_Support.Music
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Audio;
    using Discord.Commands;
    using Discord.WebSocket;

    public static class Playing
    {
        private static IAudioClient c;
        private static SocketCommandContext s;

        public static async Task StartPlaying(IAudioClient client, SocketCommandContext context, Queue q)
        {
            try
            {
                Database.Update("Music", "Playing", "Server_ID", context.Guild.Id.ToString(), true);
                s = context;
                c = client;
                Program.Client.UserVoiceStateUpdated += Client_UserVoiceStateUpdated;
                Program.Client.LoggedOut += Client_LoggedOut;
                while (true)
                {
                    q.Refresh();
                    try
                    {
                        if (q.Items[0].Type == Queue.Type.End)
                        {
                            await context.Channel.SendMessageAsync("I am done playing music");
                            q.Clear();
                            await Stop(client, context);
                            break;
                        }
                        else if (q.Items[0].Type == Queue.Type.Youtube)
                        {
                            await context.Channel.SendMessageAsync("", false, Youtube_video_embed(q.Items[0].YtVideo.ID.Replace("https://www.youtube.com/watch?v=", "")));
                            await Backend.SendUrlAsync(client, q.Items[0].YtVideo.ID);
                        }
                        else if (q.Items[0].Type == Queue.Type.Playlist)
                        {
                            // not here yet
                        }
                    }
                    finally
                    {
                        q.Items[0].Remove();
                    }
                }
                await Stop(client, context);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [StartPlaying] Task", ex);
            }
        }

        private static async Task Client_LoggedOut()
        {
            try
            {
                await Stop(c, s);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Client_LoggedOut] Task", ex);
            }
        }

        private static async Task Client_UserVoiceStateUpdated(SocketUser user, SocketVoiceState beforeState, SocketVoiceState afterState)
        {
            try
            {
                if (user.Id == 508008523146199061)
                {
                    if (afterState.VoiceChannel == null)
                    {
                        await Stop(c, s);
                    }
                }
                else
                {
                    int count = PeopleInCall(s);
                    if (count == 0)
                    {
                        await Stop(c, s);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Client_UserVoiceStateUpdated] event", ex);
            }
        }

        private static async Task Stop(IAudioClient client, SocketCommandContext context)
        {
            try
            {
                Database.Update("Music", "Playing", "Server_ID", context.Guild.Id.ToString(), false);
                await client.StopAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [stop] Task", ex);
            }
        }

        private static Embed Youtube_video_embed(string iD)
        {
            try
            {
                EmbedBuilder e = new EmbedBuilder();
                WebClient web = new WebClient();
                string text = web.DownloadString($"https://www.googleapis.com/youtube/v3/videos?part=snippet&id={iD}&key={Uri.EscapeUriString(Hidden_Info.API_Keys.Youtube)}");
                e.Title = "Now Playing: " + '"' + Json.Parse(text, "items[0].snippet.title") + '"' + " : by: " + Json.Parse(text, "items[0].snippet.channelTitle");
                e.WithThumbnailUrl(Json.Parse(text, "items[0].snippet.thumbnails.standard.url"));
                e.WithColor(255, 0, 0);
                e.WithFooter($"https://www.youtube.com/watch?v={iD}");
                return e.Build();
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [Youtube_video_embed] Embed", ex);
            }
        }

        private static int PeopleInCall(SocketCommandContext context)
        {
            try
            {
                return context.Client.GetGuild(context.Guild.Id).GetUser(508008523146199061).VoiceChannel.Users.Count - 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [PeopleInCall] int", ex);
            }
        }
    }
}
