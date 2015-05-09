//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Pipes;
//using Amazon.S3.Model;
//using Amazon.S3;
//using Amazon.S3.Transfer;
//using Infrastructure.Utilities;

//namespace Infrastructure.AWS
//{
//    public class AwsSdkRequests
//    {
//        private static int AWSErrorRetries = 3;
//        private static string UploadFileName;
//        private static string DownloadFileName;

//        #region constructors

//        public AwsSdkRequests(string fileName)
//        {
//            var util = new Utilities.Utilities();

//            UploadFileName = util.RemoveSpaces(fileName);
//            DownloadFileName = fileName;
//        }

//        #endregion

//        public bool MakePuts3UploadRequestToAws(string folderName, string bucketName)
//        {
//            var result = false;

//            if (CreateS3Folder(folderName, bucketName))
//            {
//                result = UploadFileToS3Folder(UploadFileName, folderName, bucketName);
//            }

//            return result;
//        }

//        private bool CreateS3Folder(string folderName, string bucketName)
//        {
//            var amazonS3Client = new AmazonS3Client();
//            var folderKey = folderName + "/"; //end the folder name with "/"

//            var request = new PutObjectRequest{
//                BucketName = bucketName,
//                StorageClass = S3StorageClass.Standard,
//                ServerSideEncryptionMethod = ServerSideEncryptionMethod.None,
//                //CannedACL = S3CannedACL.BucketOwnerFullControl,
//                Key = folderKey,
//                ContentBody = string.Empty
//            };

//            try
//            {
//                var response = amazonS3Client.PutObject(request);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        private bool UploadFileToS3Folder(string filePath, string folderName, string bucketName)
//        {
//            var amazonS3Client = new AmazonS3Client();
//            var transferUtil = new TransferUtility(amazonS3Client);
//            var folderKey = folderName + "/"; //end the folder name with "/"

//            var transferUtilityUploadRequest = new TransferUtilityUploadRequest
//                {
//                    BucketName = bucketName,
//                    Key = folderKey,
//                    FilePath = UploadFileName,
//                    ContentType = "audio/mpeg3"                    
//                };

//            try
//            {
//                transferUtil.Upload(transferUtilityUploadRequest);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//    }
//}