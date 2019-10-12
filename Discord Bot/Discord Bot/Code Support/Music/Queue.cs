using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public static class Queue
    {
        public enum Type
        {
            Playlist,
            Youtube,
            End,
        }

        public static class Playlist
        {
            public static string[] Songs()
            {
                return new string[] { "Not Setup yet" };
            }
        }

        public static string[] List(SocketCommandContext Context, out Type[] Out)
        {
            string RAW = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
            if (string.IsNullOrEmpty(RAW))
            {
                Out = null;
                return null;
            }
            else
            {
                RAW = RAW.Replace("|yt|", "|yt").Replace("|playlist|", "|playlist");
                string[] raw = RAW.Split('|');
                string[] output = new string[raw.Count() -1];
                Type[] Output = new Type[raw.Count() - 1];
                int current = -1;
                foreach (string Result in raw)
                {
                    if (current == -1)
                    {
                        current++;
                    }
                    else
                    {
                        if (Result.StartsWith("yt"))
                        {
                            output[current] = Result.Replace("yt", "");
                            Output[current] = Type.Youtube;
                            current++;
                        }
                        else if (Result.StartsWith("playlist"))
                        {
                            //Will not ocure Yet, this may change in the fucture.
                            output[current] = Result.Replace("playlist", "");
                            Output[current] = Type.Playlist;
                            current++;
                        }
                        else
                        {
                            output[current] = null;
                            Output[current] = Type.End;
                            current++;
                        }
                    }
                }
                Out = Output;
                return output;
            }
        }

        public static string FirstInQueue(SocketCommandContext Context, out Type Out)
        {
            Type[] temp = null;
            string Output = List(Context, out temp)[0];
            Out = temp[0];
            return Output;
        }

        public static async Task RemoveFirst(SocketCommandContext Context)
        {
            Type type;
            string remove = FirstInQueue(Context, out type);
            if (type == Type.Youtube)
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue").Remove(0, 14));
            }
            else if (type == Type.Playlist)
            {
                //not here yet
            }
            else
            {
                await Clear(Context);
            }
        }

        public static async Task Clear(SocketCommandContext Context)
        {
            await Context.Channel.SendMessageAsync("I am done playing music");
            Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), "");
        }

        public static async Task AddSongToQueue(SocketCommandContext Context, string ID)
        {
            string old = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
            if (!string.IsNullOrEmpty(old))
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), old.Replace("|End|", "") + $"|yt|{ID}|End|");
            }
            else
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), $"|yt|{ID}|End|");
            }
        }
    }
}
