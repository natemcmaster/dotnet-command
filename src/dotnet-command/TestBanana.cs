using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Library
{
    public class BananaSql
    {
        public BananaSql(string connStr)
        {
            JsonConvert.SerializeObject(new object());
            
            using(var connection = new SqlConnection(connStr))
            {
                connection.Open();
                Console.WriteLine($"Opened connection to {connection.ConnectionString} successfully");
                Console.WriteLine("Server version = " + connection.ServerVersion);
            }
        }
    }
}