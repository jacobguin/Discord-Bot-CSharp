using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Code_Support.Music
{
    public static class Backend
    {
        private static AudioOutStream discord;
        private static Stream output;

        private static Process YTStreame(string url)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C youtube-dl.exe --no-check-certificate -f bestaudio -o - {url} | ffmpeg.exe -i pipe:0 -f s16le -ar 48000 -ac 2 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            });
        }

        public static async Task SendUrlAsync(IAudioClient client, string url)
        {
            using (Process ffmpeg = YTStreame(url))
            {
                output = ffmpeg.StandardOutput.BaseStream;
                discord = client.CreatePCMStream(AudioApplication.Mixed);
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}
