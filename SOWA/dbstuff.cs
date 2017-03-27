using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace SOWA
{
    public class dbstuff
    {

        private static MySqlConnection con = null;
        private static int lastInsertID = -1;

        public static MySqlConnection getDBConection()
        {
            if (con == null)
            {
                string connectionString = "Server=localhost;Database=alarm;Uid=sid;Pwd=Password1";
                con = new MySqlConnection(connectionString);
            }

            return con;
        }

        public static bool createSystemStateChangeEvent(int idhome, int state)
        {
            bool success = false;

            MySqlConnection con = getDBConection();

            MySqlCommand cmd = new MySqlCommand(@"
INSERT INTO homelog (
	idhome,
	timestamp,
    state
) VALUES (
	@id,
	@ts,
    @state
)", con);

            MySqlParameter paramidhome = new MySqlParameter("@id", MySqlDbType.Int32);
            MySqlParameter paramTimestamp = new MySqlParameter("@ts", MySqlDbType.DateTime);
            MySqlParameter paramState = new MySqlParameter("@state", MySqlDbType.Enum);

            paramidhome.Value = idhome;
            paramTimestamp.Value = DateTime.Now;
            paramState.Value = state;

            cmd.Parameters.Add(paramidhome);
            cmd.Parameters.Add(paramTimestamp);
            cmd.Parameters.Add(paramState);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                success = true;
            }
            catch (MySqlException MySqlE)
            {
                throw MySqlE;
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        //        public static bool createNewNotification(int roomID)
        //        {
        //            bool success = false;

        //            MySqlConnection con = getDBConection();

        //            MySqlCommand cmd = new MySqlCommand(@"
        //INSERT INTO notifications (
        //	timestamp,
        //	roomID
        //) VALUES (
        //	@ts,
        //	@room
        //)", con);

        //            MySqlParameter paramTimestamp = new MySqlParameter("@ts", MySqlDbType.DateTime);
        //            MySqlParameter paramRoomID = new MySqlParameter("@room", MySqlDbType.Int32);

        //            paramTimestamp.Value = DateTime.Now;
        //            paramRoomID.Value = roomID;

        //            cmd.Parameters.Add(paramTimestamp);
        //            cmd.Parameters.Add(paramRoomID);

        //            try
        //            {
        //                con.Open();
        //                cmd.ExecuteNonQuery();

        //                // http://stackoverflow.com/questions/15373851/c-sharp-get-insert-id-with-auto-increment
        //                lastInsertID = (int)cmd.LastInsertedId;
        //                success = true;
        //            }
        //            catch (MySqlException MySqlE)
        //            {
        //                throw MySqlE;
        //            }
        //            finally
        //            {
        //                con.Close();
        //            }

        //            return success;
        //        }

        public static bool createReading(int sensorID, int idrooms, string SensorValue)
        {
            bool success = false;

            MySqlConnection con = getDBConection();

            MySqlCommand cmd = new MySqlCommand(@"
INSERT INTO sensorlog (
    idsensor,
    idrooms,
    timestamp,
    sensorState
) VALUES (

    @idsensor,
    @idrooms,
    @timestamp,
    @sensorState
)", con);

            MySqlParameter paramSensorID = new MySqlParameter("@idsensor", MySqlDbType.Int32);
            MySqlParameter paramRoomID = new MySqlParameter("@idrooms", MySqlDbType.Int32);
            MySqlParameter paramTimestamp = new MySqlParameter("@timestamp", MySqlDbType.DateTime);

            MySqlParameter paramSensorValue = new MySqlParameter("@sensorState", MySqlDbType.VarChar);

            paramRoomID.Value = idrooms;
            paramSensorID.Value = sensorID;
            paramTimestamp.Value = DateTime.Now;
            paramSensorValue.Value = SensorValue;

            cmd.Parameters.Add(paramRoomID);
            cmd.Parameters.Add(paramSensorID);
            cmd.Parameters.Add(paramTimestamp);

            cmd.Parameters.Add(paramSensorValue);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                success = true;
            }
            catch (MySqlException MySqlE)
            {
                throw MySqlE;
            }
            finally
            {
                con.Close();
            }

            return success;
        }

        //public static bool submitReadingAndCreateGroup(int roomID, int sensorID, string value, string notification)
        //{
        //    // establish whether this is creating a new notification or adding to an existing one
        //    if (notificationID == -1)
        //    {
        //        bool groupCreated = createNewNotification(roomID);
        //        int newNotificationID = lastInsertID;
        //        Console.WriteLine($"new notification: {newNotificationID}");

        //        if (groupCreated)
        //        {
        //            return createReading(sensorID, value, newNotificationID);
        //        }
        //        else {
        //            Console.WriteLine("Error: Notification group could not be created.");
        //            return false;
        //        }
        //    }
        //    else {
        //        return createReading(sensorID, value, notificationID);
        //    }
        //}

        public static bool toggleAlarmState(int homeID, bool on)
        {
            bool success = false;

            MySqlConnection con = getDBConection();

            MySqlCommand cmd = new MySqlCommand(@"
UPDATE home 
SET alarmS = @state
WHERE idhome = @id
", con);

            MySqlParameter paramState = new MySqlParameter("@state", MySqlDbType.Int32);
            MySqlParameter paramID = new MySqlParameter("@id", MySqlDbType.Enum);

            int state = (on) ? 1 : 2;

            paramState.Value = state;
            paramID.Value = homeID;

            cmd.Parameters.Add(paramState);     
            cmd.Parameters.Add(paramID);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                success = true;
            }
            catch (MySqlException MySqlE)
            {
                throw MySqlE;
            }
            finally
            {
                con.Close();
            }

            if (success)
            {
                // log the change in state
                success = createSystemStateChangeEvent(homeID, state);
            }

            return success;
        }

        public static string getHomeState(int homeID)
        {
            MySqlConnection con = getDBConection();

            MySqlCommand cmd = new MySqlCommand(@"
SELECT alarmS
FROM home
WHERE idhome = @home
", con);

            MySqlParameter paramID = new MySqlParameter("@home", MySqlDbType.Int32);

            paramID.Value = homeID;
            cmd.Parameters.Add(paramID);

            string state = "unknown";

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    state = reader.GetString(0);
                }
                else {
                    Console.WriteLine(String.Format("System state for home '{0}' not found.", homeID));
                }

                reader.Close();
            }
            catch (MySqlException MySqlE)
            {
                throw MySqlE;
            }
            finally
            {
                con.Close();
            }

            return state;
        }

        public static XmlDocument getSensorLogsRoom(int idrooms, DateTime start, DateTime end)
        {
            MySqlConnection con = getDBConection();

            MySqlCommand cmd = new MySqlCommand(@"
SELECT * FROM sensorlog WHERE idrooms = @idroom
    AND timestamp BETWEEN @start AND @end
", con);

            MySqlParameter paramRoomID = new MySqlParameter("@idroom", MySqlDbType.Int32);
            MySqlParameter paramStartTime = new MySqlParameter("@start", MySqlDbType.DateTime);
            MySqlParameter paramEndTime = new MySqlParameter("@end", MySqlDbType.DateTime);

            paramRoomID.Value = idrooms;
            paramStartTime.Value = start;
            paramEndTime.Value = end;

            cmd.Parameters.Add(paramRoomID);
            cmd.Parameters.Add(paramStartTime);
            cmd.Parameters.Add(paramEndTime);

            XmlDocument xmlDom = new XmlDocument();
            xmlDom.AppendChild(xmlDom.CreateElement("sensorlogs"));

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                MySqlDataAdapter dataAdptr = new MySqlDataAdapter();
                dataAdptr.SelectCommand = cmd;
                DataSet ds = new DataSet("sensorlogs");
                dataAdptr.Fill(ds, "sensorlogs");

                xmlDom.LoadXml(ds.GetXml());

            }
            catch (MySqlException MySqlE)
            {
                throw MySqlE;
            }
            finally
            {
                con.Close();
            }


            return xmlDom;
        }

    }
}