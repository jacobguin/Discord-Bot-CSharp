namespace Discord_Bot
{
    using System;
    using System.Windows.Forms;
    using Discord.Commands;
    using Discord.WebSocket;

    public static class Program
    {
        public static DiscordSocketClient Client;
        public static CommandService Commands;
        public static MainForm MF;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Load());
        }
    }
}
