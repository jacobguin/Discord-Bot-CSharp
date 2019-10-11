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
            Youtube
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
                string[] output = null;
                Type[] Output = null;
                int current = 0;
                foreach (string Result in raw)
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
    }
}
