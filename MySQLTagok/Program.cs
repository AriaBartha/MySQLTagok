using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQLTagok
{
    internal class Program
    {
        static List<Tagok> taglista = new List<Tagok>();
        static MySqlConnection connection = null;
        static MySqlCommand command = null;
        static void Main(string[] args)
        {
            beolvasas();
            tagListazas();
            ujTagFelvetel(1014,"Gipsz Jakab",1960,2000,"H");
            ujTagFelvetel(1015, "Minta János", 1970, 1014, "H");
            tagTorlese(1014);            
            Console.WriteLine("\nProgram vége.");
            Console.ReadLine();
        }

        private static void tagTorlese(int azon)
        {
            
            command.CommandText = "DELETE FROM `ugyfel` WHERE azon = @azonSz";
            command.Parameters.AddWithValue("@azonSz", azon);          

             try
             {
                 if (connection.State != System.Data.ConnectionState.Open)
                 {
                     connection.Open();
                 }


                 command.ExecuteNonQuery();
                 connection.Close();
             }
             catch (MySqlException ex)
             {
                 Console.WriteLine(ex.Message);
                 Environment.Exit(0);
             }
        }

        private static void ujTagFelvetel(int azon,string nev,int szulev,int irszam, string orsz)
        {
            
            command.CommandText = "INSERT INTO `ugyfel`(`azon`, `nev`, `szulev`, `irszam`, `orsz`) VALUES (@azon,@nev,@szulev,@irszam,@orsz)";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@azon", azon);
            command.Parameters.AddWithValue("@nev", nev);
            command.Parameters.AddWithValue("@szulev", szulev);
            command.Parameters.AddWithValue("@irszam", irszam);
            command.Parameters.AddWithValue("@orsz", orsz);
            try
            {
                if(connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
               
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        private static void tagListazas()
        {
            foreach (var tag in taglista) 
            { 
                Console.WriteLine(tag);
            }
        }

        private static void beolvasas()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Clear();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "";
            sb.Database = "tagdij";
            sb.CharacterSet = "utf8";
            connection = new MySqlConnection(sb.ConnectionString);
            command = connection.CreateCommand();
            try
            {
                connection.Open();                
                command.CommandText = "SELECT * FROM `ugyfel`";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tagok tag = new Tagok(reader.GetInt32("azon"), reader.GetString("nev"), reader.GetInt32("szulev"), reader.GetInt32("irszam"), reader.GetString("orsz"));
                        taglista.Add(tag);
                    }
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            
        }
    }
}
