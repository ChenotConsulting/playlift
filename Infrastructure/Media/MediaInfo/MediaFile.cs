using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Media.MediaInfo
{
    public class MediaFile
    {
        #region private members

        private readonly int Duration;
        private readonly int FileSize;

        public MediaFile(string filePath)
        {
            var mediaInfo = new MediaInfo();
            mediaInfo.Open(filePath);

            Duration = (string.IsNullOrEmpty(mediaInfo.Get(0, 0, "Duration")) ? 0 : Convert.ToInt32(mediaInfo.Get(0, 0, "Duration")));
            FileSize = (string.IsNullOrEmpty(mediaInfo.Get(0, 0, "FileSize")) ? 0 : Convert.ToInt32(mediaInfo.Get(0, 0, "FileSize")));
        }

        public int GetDuration()
        {
            return Duration;
        }

        public int GetFileSize()
        {
            return FileSize;
        }

        #endregion
    }
}
