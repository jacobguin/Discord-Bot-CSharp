using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord_Bot.Events;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private async void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            await Program.Client.LogoutAsync();
            Application.Exit();
        }
    }
}
