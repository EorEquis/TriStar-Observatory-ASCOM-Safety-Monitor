using System;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using ASCOM.Utilities;

namespace ASCOM.LocalServer
{
    class Weather
    {
        internal static TraceLogger tlwx;

        public string LastWrite { get; set; }
        public int Alert { get; set; }
        public int csFailCount = 0;


        public void checkSafety(string URL)
        {
            Weather updatedWeather = new Weather();
            try
            {
                // wx = New Weather
                var webclient = new Utils.WebClient();
                string response = webclient.DownloadString(URL);
                updatedWeather = JsonConvert.DeserializeObject<LocalServer.Weather>(response);
                tlwx.LogMessage("checkSafety", "Read JSON at " + DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") + " UTC");
                tlwx.LogMessage("checkSafety", "LastWrite at " + Convert.ToDateTime(updatedWeather.LastWrite).ToString("yyyy-MM-dd HH:mm:ss") + " UTC");
                this.Alert = updatedWeather.Alert;
                if (DateAndTime.DateDiff(DateInterval.Minute, Convert.ToDateTime(updatedWeather.LastWrite), DateTime.Now.ToUniversalTime()) > 5L)
                {
                    this.Alert = 1;
                    tlwx.LogMessage("checkSafety ERROR", "Unsafe set, LastWrite too old!");
                }
                tlwx.LogMessage("checkSafety", "Alert " + this.Alert.ToString());
                csFailCount = 0;
            }
            catch (Exception ex)
            {
                csFailCount++;
                if (csFailCount > 2)
                {
                    this.Alert = 1;
                    tlwx.LogMessage("checkSafety ERROR", ex.Message);
                }
                // Throw New DriverException("checkSafety failed.  Check log for details")
            }
        }
    }
}
