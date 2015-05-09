using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.AWS;

namespace Infrastructure.Delivery
{
    public class S3FileDelivery : IFileDelivery
    {
        private string UploadFileName;
        private string DownloadFileName;
        private string FolderName;
        private string BucketName;
        private Utilities.Utilities util;

        #region constructors

        public S3FileDelivery()
        {
            util = new Utilities.Utilities();
        }

        public S3FileDelivery(string filePath)
        {
            UploadFileName = filePath;
            DownloadFileName = filePath;
            util = new Utilities.Utilities();
        }

        public S3FileDelivery(string filePath, string folderName)
        {
            UploadFileName = filePath;
            FolderName = folderName;
            util = new Utilities.Utilities();
        }

        #endregion

        public bool UploadFile(string filePath, string uploadType)
        {
            var isUploaded = false;
            var mySettings = new Properties.Settings();

            // apply the correct bucket
#if DEBUG
            if (uploadType == "master")
            {
                BucketName = mySettings.MasterBucketNameTest;
            }
            else
            {
                BucketName = mySettings.UploadBucketNameTest;
            }
#else
            if (uploadType == "master")
            {
                BucketName = mySettings.MasterBucketName;
            }
            else
            {
                BucketName = mySettings.UploadBucketName;
            }
#endif

            if (string.IsNullOrEmpty(UploadFileName))
            {
                UploadFileName = filePath;
            }

            try
            {
                // instantiate the upload class and make the call to upload the file
                var s3FileUpload = new AWSRequests(UploadFileName);

                isUploaded = s3FileUpload.MakePUTS3UploadRequestToAws(false, BucketName, FolderName);

                if (isUploaded)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
            }

            return isUploaded;
        }

        public bool DownloadFile(string filePath, string keyName)
        {
            var isDownloaded = false;
            var mySettings = new Properties.Settings();

            if (string.IsNullOrEmpty(DownloadFileName))
            {
                DownloadFileName = Path.Combine(filePath, keyName);
            }

            try
            {
                // instantiate the upload class and make the call to upload the file
                var s3FileDownload = new AWSRequests(DownloadFileName);

                isDownloaded = s3FileDownload.MakeGETDownloadRequestToAws(true, "trmmaster", keyName, "");
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
            }

            return isDownloaded;
        }
    }
}
