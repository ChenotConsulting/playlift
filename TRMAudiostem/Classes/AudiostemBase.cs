using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using TRMAudiostem.TRMWebServiceJson;

namespace TRMAudiostem
{
    public static class AudiostemBase
    {
        public static string StreamingUrl
        {
            get
            {
#if DEBUG
                return ConfigurationManager.AppSettings["AWSS3UrlTest"];
#else
                return ConfigurationManager.AppSettings["AWSS3Url"];
#endif
            }
        }

        public static string LocalTempDestinationPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalTempFilePath"].ToString(CultureInfo.InvariantCulture);
            }
        }

        public static string CloudfrontUrl
        {
            get
            {
#if DEBUG
                return ConfigurationManager.AppSettings["AWSS3UrlTest"].ToString(CultureInfo.InvariantCulture);
#else
                return ConfigurationManager.AppSettings["AWSS3Url"].ToString(CultureInfo.InvariantCulture);
#endif
            }
        }

        public static string SaveFileLocally(System.Web.HttpPostedFileBase sourceFile)
        {
            if (!Directory.Exists(LocalTempDestinationPath))
            {
                Directory.CreateDirectory(LocalTempDestinationPath);
            }

            var localFile = Path.Combine(LocalTempDestinationPath, sourceFile.FileName);

            if (!File.Exists(localFile))
            {
                sourceFile.SaveAs(localFile);
            }

            return localFile;
        }

        public static string SerializeObjectToJson(object obj)
        {
            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(obj);
            return json;
        }

        public static string ReturnFormItemValue(string[] form, string key)
        {
            string value = string.Empty;
            if (!string.IsNullOrEmpty(form.FirstOrDefault(x => x.ToString().Contains(key))))
            {
                value = form.FirstOrDefault(x => x.ToString().Contains(key)).ToString().Split('=')[1];
            }

            return value.Replace("+", " ").Replace("%2F", "/").Replace("%3F", "?").Replace("%3D", "=");
        }
    }
}