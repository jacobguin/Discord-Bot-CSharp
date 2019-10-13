using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Discord_Bot
{
    class Program
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
