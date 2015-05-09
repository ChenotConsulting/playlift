using System.Configuration;
using System.Globalization;
using System.ServiceModel;

namespace WebService.Classes
{
    public class BaseClass
    {
        protected static string ConnectionString
        {
            get
            {
#if DEBUG
                return ConfigurationManager.ConnectionStrings["testConnString"].ToString();
#else
                return ConfigurationManager.ConnectionStrings["liveConnString"].ToString();
#endif
            }
        }

        protected static string AudioDestinationPath
        {
            get
            {
                return ConfigurationManager.AppSettings["InPutFilePath"].ToString(CultureInfo.InvariantCulture);
            }
        }

        protected static string LocalTempDestinationPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalTempFilePath"].ToString(CultureInfo.InvariantCulture);
            }
        }

        protected static string LocalDownloadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalDownloadPath"].ToString(CultureInfo.InvariantCulture);
            }
        }

        protected static string LocalDownloadUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["LocalDownloadUrl"].ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}