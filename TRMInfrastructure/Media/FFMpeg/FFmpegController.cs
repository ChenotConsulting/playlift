using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using FFMpegController.Classes;
using Newtonsoft.Json;
using TRMInfrastructure.Media.MediaInfo;

namespace TRMInfrastructure.Media.FFMpeg
{
    public static class FFmpegController
    {
        private static readonly string InPutFilePath = ConfigurationManager.AppSettings["InPutFilePath"].ToString(CultureInfo.InvariantCulture);
        private static readonly string OutPutFilePath = ConfigurationManager.AppSettings["OutPutFilePath"].ToString(CultureInfo.InvariantCulture);
        private static readonly string FfMpegPath = ConfigurationManager.AppSettings["FFMpegPath"].ToString(CultureInfo.InvariantCulture);
        private static readonly string ThumbnailsPath = ConfigurationManager.AppSettings["ThumbnailsPath"].ToString(CultureInfo.InvariantCulture);
        private static readonly string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"].ToString(CultureInfo.InvariantCulture) + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + DateTime.Now.ToShortTimeString().Replace(":", "") + ".rtf";

        public static string EncodeAudio(string localFile, string ext, string destinationFile)
        {
            const string audioBitRate = "128";

            var encoder = new AudioEncoder { FFmpegPath = FfMpegPath };

            // select the commands depending on the file extension requested
            string command;
            switch (ext)
            {
                case "mp3":
                    command = QuickAudioEncodingCommands.MP3128Kbps;
                    break;
                case "aac":
                    command = QuickAudioEncodingCommands.AAC128Kbps;
                    break;
                case "wav":
                default:
                    command = QuickAudioEncodingCommands.WAV128Kbps;
                    break;
            }

            // set up the parameters that we will use for the encoding
            var parameters = new AudioEncoderParameters
            {
                InputPath = localFile,
                EncodingCommandPass = command,
                OutPutFile = OutPutFilePath + Path.GetFileName(destinationFile),
                Suffix = audioBitRate,
                Extension = ext
            };

            // if the destination file exists, then delete it or FFmpeg will freeze
            // first we must recreate the final file path as it is created in the transcoder
            var destinationFilePath = Path.GetDirectoryName(parameters.OutPutFile);
            var destinationFileName = Path.GetFileNameWithoutExtension(parameters.OutPutFile);
            destinationFileName = Path.Combine(destinationFilePath, string.Format("{0}_128.{1}", destinationFileName, ext));

            if (File.Exists(destinationFileName))
            {
                File.Delete(destinationFileName);
            }

            var json = JsonConvert.SerializeObject(parameters);
            // encode the file
            var encoded = JsonConvert.DeserializeObject<EncodedAudio>(encoder.EncodeAudio(json));

            if (encoded.Success)
            {
                destinationFile = encoded.EncodedAudioPath;
                File.Delete(localFile);
            }
            else
            {
                using (var fs = File.Create(LogFilePath))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(encoded.EncodingLog);

                    var info = new UTF8Encoding(true).GetBytes(sb.ToString());
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                destinationFile = "error";
            }

            return destinationFile;
        }
    }
}