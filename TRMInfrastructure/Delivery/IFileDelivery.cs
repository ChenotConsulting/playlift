using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMInfrastructure.Delivery
{
    interface IFileDelivery
    {
        bool UploadFile(string filePath, string uploadType);
        bool DownloadFile(string filePath, string keyName);
    }
}
