using Discord;
using System;
using System.Threading.Tasks;

namespace Discord_Bot.Events
{
    public static class Log
    {
        public static void Load()
        {
            Program.Client.Log += Client_Log;
        }

        private static async Task Client_Log(LogMessage Message)
        {
            Program.MF.AddText($"[{DateTime.Now} at {Message.Source}] {Message.Message}", System.Drawing.Color.Gold);
        }
    }
}
