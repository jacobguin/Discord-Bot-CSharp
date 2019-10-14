using FileTransferProtocalLibrary;
using MetroFramework.Forms;
using System;
using System.Windows.Forms;

namespace Discord_Bot
{
    public partial class MainForm : MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void AddText(string Text, System.Drawing.Color Color)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = Color;
            richTextBox1.AppendText(Text + Environment.NewLine);
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FTP ftp = new FTP($"ftp://{Hidden_Info.Ftp.Domain}/Jacob/Program%20Files/Bot/", Hidden_Info.Ftp.Username, Hidden_Info.Ftp.Password);
            ftp.DeleteFile("Bot.accdb");
            ftp.UploadFile("Bot.accdb", $"C:/Users/{Environment.UserName}/Documents/Bot.accdb");

            await Program.Client.LogoutAsync();
            Application.Exit();
        }
    }
}
