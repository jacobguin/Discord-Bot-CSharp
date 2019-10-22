namespace Discord_Bot.Code_Support.Music
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Discord.Audio;

    public static class Backend
    {
        private static AudioOutStream discord;
        private static Stream output;

        public static async Task SendUrlAsync(IAudioClient client, string url)
        {
            try
            {
                using (Process ffmpeg = YTStream(url))
                {
                    output = ffmpeg.StandardOutput.BaseStream;
                    discord = client.CreatePCMStream(AudioApplication.Mixed);
                    try
                    {
                        await output.CopyToAsync(discord);
                    }
                    finally
                    {
                        await discord.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [SendUrlAsync] Task", ex);
            }
        }

        private static Process YTStream(string url)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Something Went wrong in the [YTStream] Process", ex);
            }
        }
    }
}
