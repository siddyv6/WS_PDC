using System.Web.Services;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

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
    public class sowa : System.Web.Services.WebService
    {

        //[WebMethod]
        //public bool createNewNotification(int roomID) {
        //    return dbstuff.createNewNotification(roomID);
        //}

        [WebMethod]
        public bool createSensorReadingAndLog(int sensorID, int idrooms, string SensorValue)
        {
            return dbstuff.createReading(sensorID, idrooms, SensorValue);
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

        [WebMethod]
        public XmlDocument getSensorLogsTime(int idroom, DateTime start, DateTime end)
        {
            return dbstuff.getSensorLogsRoom(idroom, start, end);
        }
    }
}
