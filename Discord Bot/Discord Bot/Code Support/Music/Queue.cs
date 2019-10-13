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

        public static Queue_Item[] List(SocketCommandContext Context)
        {
            string RAW = Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue");
            if (string.IsNullOrEmpty(RAW))
            {
                return null;
            }
            else
            {
                RAW = RAW.Replace("|yt|", "|yt").Replace("|playlist|", "|playlist");
                string[] raw = RAW.Split('|');
                Queue_Item[] Output = new Queue_Item[raw.Count() - 2];
                int current = -1;
                foreach (string Result in raw)
                {
                    if (current == -1)
                    {
                        current++;
                    }
                    else if (current == raw.Count() - 2)
                    {
                        current++;
                    }
                    else
                    {
                        if (Result.StartsWith("yt"))
                        {
                            Output[current] = new Queue_Item(Type.Youtube, Result.Replace("yt", ""));
                            current++;
                        }
                        else if (Result.StartsWith("playlist"))
                        {
                            //Will not ocure Yet, this may change in the fucture.
                            Output[current] = new Queue_Item(Type.Playlist, Result.Replace("playlist", ""));
                            current++;
                        }
                        else
                        {
                            Output[current] = new Queue_Item(Type.End, null);
                            current++;
                        }
                    }
                }
                return Output;
            }
        }

        public static Queue_Item FirstInQueue(SocketCommandContext Context)
        {
            return List(Context)[0];
        }

        public static async Task RemoveFirst(SocketCommandContext Context)
        {
            Queue_Item remove = FirstInQueue(Context);
            if (remove.Type == Type.Youtube)
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue").Remove(0, 14));
            }
            else if (remove.Type == Type.Playlist)
            {
                //not here yet
            }
            else
            {
                await Clear(Context);
            }
        }

        public class Queue_Item
        {
            public Queue_Item(Type type, string Result)
            {
                Type = type;
                Video = Result;
            }

            public Type Type { get; }

            public string Video { get; }
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
                if (Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Playing") == null)
                {
                    Database.Write("Music", "Server_ID", Context.Guild.Id.ToString());
                }
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), $"|yt|{ID}|End|");
            }
        }
    }
}
