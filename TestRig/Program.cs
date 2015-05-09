using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRig
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var locationService = new net.webservicex.www.UKLocation();
            //locationService.UseDefaultCredentials = true;
            //locationService.Timeout = 60000;
            //var result = locationService.GetUKLocationByPostCode("n11");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UserManagement());
        }
    }
}
