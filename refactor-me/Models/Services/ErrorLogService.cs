using refactor_me.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace refactor_me.Models.Services
{
    public class ErrorLogService
    {
        public static void LogError(Exception ex)
        {
            var config = new ConfigurationManagerWapper();
            var filePath = HttpContext.Current.Server.MapPath (config.GetAppSettingValue(Constants.LogPath));

            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                                 "" + Environment.NewLine + "Date :" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}