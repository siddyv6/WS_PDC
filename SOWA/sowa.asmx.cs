using System.Web.Services;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOWA
{
    /// <summary>
    /// Summary description for WebServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServices : System.Web.Services.WebService
    {

        //[WebMethod]
        //public bool createNewNotification(int roomID) {
        //    return dbstuff.createNewNotification(roomID);
        //}

        [WebMethod]
        public bool createReading(int sensorID, int idrooms, string SensorValue, string notification)
        {
            return dbstuff.createReading(sensorID, idrooms, SensorValue, notification);
        }

        //[WebMethod]
        //public bool submitReadingAndCreateGroup(int roomID, int sensorID, string value, int notificationID = -1) {
        //    return dbstuff.submitReadingAndCreateGroup(roomID, sensorID, value, notificationID);
        //}

        [WebMethod]
        public string checkHome(int homeID)
        {
            return dbstuff.getHomeState(homeID);
        }

        [WebMethod]
        public bool resetSystem(int homeID)
        {
            return dbstuff.toggleAlarmState(homeID, false);
            //return dbstuff.toggleAlarmState(homeID, true);
        }

        [WebMethod]
        public bool toogleSystem(int homeID, bool on)
        {
            return dbstuff.toggleAlarmState(homeID, on);
        }
    }
}
