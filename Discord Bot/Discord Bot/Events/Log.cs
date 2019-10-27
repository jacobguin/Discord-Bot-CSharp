namespace Discord_Bot.Events
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Discord;

    public class Log
    {
        public Log()
        {
            Program.Client.Log += Client_Log;
        }

        private static async Task Client_Log(LogMessage message)
        {
            try
            {
                if (Program.MF.InvokeRequired == true)
                    Program.MF.Invoke(new MethodInvoker(() => { Program.MF.AddText($"[{DateTime.Now} at {message.Source}] {message.Message}", System.Drawing.Color.Gold); }));
                else
                    Program.MF.AddText($"[{DateTime.Now} at {message.Source}] {message.Message}", System.Drawing.Color.Gold);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("cannot be called on a controleuntil the window handle has been created"))
                {
                    MessageBox.Show("error:" + ex.Message);
                }
            }
        }
    }
}
