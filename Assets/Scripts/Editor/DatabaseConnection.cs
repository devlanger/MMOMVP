using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace WebSocketMMOServer.Database
{
    public class DatabaseManager
    {
        public static string connectionString = "";

        public DatabaseManager()
        {
            try
            {
                XDocument configXml = XDocument.Load("Assets/db.xml");

                string hostname = configXml.Root.Element("hostname").Value;
                string username = configXml.Root.Element("username").Value;
                string password = configXml.Root.Element("password").Value;
                string port = configXml.Root.Element("port").Value;
                string database = configXml.Root.Element("database").Value;

                connectionString = string.Format("Server={0};Port={1};Database={2};User={3};Password={4};SslMode=none; convert zero datetime=True", hostname, port, database, username, password);
                Console.WriteLine("Loaded database.");
            }
            catch (Exception ex)
            {
                Debug.Log("Couldnt load db config: " + ex.ToString());
            }
        }

        public static DataTable ReturnQuery(string query)
        {
            DataTable results = new DataTable("Result");
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        results.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(query + " /// Return query error: " + ex.ToString());
                }
            }

            return results;
        }

        public static int GetLastInsertedId(string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string lastInsertedQuestQuery = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'izommo' AND TABLE_NAME = '" + tableName + "'";
                MySqlCommand command = new MySqlCommand(lastInsertedQuestQuery, conn);
                command.ExecuteNonQuery();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static long InsertQuery(string query)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                return command.LastInsertedId;
            }
        }
    }
}
