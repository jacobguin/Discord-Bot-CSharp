using Discord.Commands;
using System.Linq;
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
                string[] output = new string[raw.Count() - 1];
                Type[] Output = new Type[raw.Count() - 1];
                for (int i = -1; i < raw.Length - 1; i++)
                {
                    string Result = raw[i];
                    if (Result.StartsWith("yt"))
                    {
                        output[i] = Result.Replace("yt", "");
                        Output[i] = Type.Youtube;
                    }
                    else if (Result.StartsWith("playlist"))
                    {
                        //Will not occur Yet, this may change in the future.
                        output[i] = Result.Replace("playlist", "");
                        Output[i] = Type.Playlist;
                    }
                    else if (i != -1)
                    {
                        output[i] = null;
                        Output[i] = Type.End;
                    }
                }
                Out = Output;
                return output;
            }
        }

        public static string FirstInQueue(SocketCommandContext Context, out Type Out)
        {
            string Output = List(Context, out Type[] temp)[0];
            Out = temp[0];
            return Output;
        }

        public static async Task RemoveFirst(SocketCommandContext Context)
        {
            _ = FirstInQueue(Context, out Type type);
            if (type == Type.Youtube)
            {
                Database.Update("Music", "Queue", "Server_ID", Context.Guild.Id.ToString(), Database.Read("Music", "Server_ID", Context.Guild.Id.ToString(), "Queue").Remove(0, 14));
            }
            else if (type == Type.Playlist)
            {
                // wip
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

        public static void AddSongToQueue(SocketCommandContext Context, string ID)
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
