using System;
using System.Collections.Generic;
using System.IO;

namespace TRMInfrastructure.AWS
{
    public class AWSRequests
    {
        private static string RequestMethod;
        private static string RequestURL;
        private static Dictionary<String, String> ExtraRequestHeaders;
        private static int AWSErrorRetries = 3;
        private static string UploadFileName;
        private static string DownloadFileName;
        private Utilities.Utilities util;

        #region constructors

        public AWSRequests(string fileName)
        {
            UploadFileName = fileName;
            DownloadFileName = fileName;
            util = new Utilities.Utilities();
        }

        #endregion

        public bool MakeGETDownloadRequestToAws(bool useSSL, string bucketName, string keyName, string queryString)
        {
            var result = false;
            var mySettings = new Properties.Settings();

            //create an instance of the REST class
            var myDownload = new SprightlySoftAWS.S3.Download();

            // build the URL to call. The bucket name and key name must be empty to return all buckets
            RequestURL = myDownload.BuildS3RequestURL(useSSL, "s3.amazonaws.com", bucketName, keyName, queryString);

            RequestMethod = "GET";

            ExtraRequestHeaders = new Dictionary<String, String> {
                {
                    "x-amz-date", DateTime.UtcNow.ToString("r")
                }};
            //add a date header

            //generate the authorization header value
            var authorizationValue = myDownload.GetS3AuthorizationValue(RequestURL, RequestMethod, ExtraRequestHeaders, "AKIAJCHWEMYQ5UXHOWSQ", "GLQ3fzqhpJmEvRf3J6SWquH1KbQdVEI+3tqqWGiy");
            //add the authorization header
            if (!ExtraRequestHeaders.ContainsValue(authorizationValue))
            {
                ExtraRequestHeaders.Add("Authorization", authorizationValue);
            }

            //call Download file to submit the download request
            result = myDownload.DownloadFile(RequestURL, RequestMethod, ExtraRequestHeaders, DownloadFileName, true);

            return result;
        }

        public bool MakePUTS3UploadRequestToAws(bool useSSL, string bucketName, string folderName)
        {
            var result = false;
            var mySettings = new Properties.Settings();

            try
            {
                //create an instance of the REST class
                var myUpload = new SprightlySoftAWS.S3.Upload();

                // build the URL to call. The bucket name and key name must be empty to return all buckets
                RequestURL = myUpload.BuildS3RequestURL(useSSL, "s3.amazonaws.com", bucketName, folderName + Path.GetFileName(UploadFileName), string.Empty);

                RequestMethod = "PUT";

                //add a date header
                ExtraRequestHeaders = new Dictionary<String, String> {
                {
                    "x-amz-date", DateTime.UtcNow.ToString("r")
                }};

                //generate the authorization header value
                var authorizationValue = myUpload.GetS3AuthorizationValue(RequestURL, RequestMethod, ExtraRequestHeaders, mySettings.AWSAccessKeyId, mySettings.AWSSecretAccessKey);
                // add the public access permission header
                //ExtraRequestHeaders.Add("x-amz-acl", "public-read");
                //add the authorization header
                ExtraRequestHeaders.Add("Authorization", authorizationValue);

                //call UploadFile to submit the upload request
                result = myUpload.UploadFile(RequestURL, RequestMethod, ExtraRequestHeaders, UploadFileName);

                var requestResult = myUpload.LogData;

                if (result)
                {
                    util.LogMessage("UploadSuccess", requestResult);
                }
                else
                {
                    util.LogMessage("UploadFailure", requestResult);
                }
            }
            catch (Exception ex)
            {
                util.ErrorNotification(ex);
            }

            return result;
        }
    }
}
