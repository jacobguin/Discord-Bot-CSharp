namespace Discord_Bot.Events
{
    using System;
    using System.Threading.Tasks;
    using Discord;

    public static class Log
    {
        public static void Load()
        {
            Program.Client.Log += Client_Log;
        }

        private static async Task Client_Log(LogMessage message)
        {
            Program.MF.AddText($"[{DateTime.Now} at {message.Source}] {message.Message}", System.Drawing.Color.Gold);
        }
    }
}
