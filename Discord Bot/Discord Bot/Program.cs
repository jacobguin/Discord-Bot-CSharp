using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Windows.Forms;

namespace Discord_Bot
{
    public static class Program
    {
        public static DiscordSocketClient Client;
        public static CommandService Commands;
        public static MainForm MF;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Load());
        }
    }
}
