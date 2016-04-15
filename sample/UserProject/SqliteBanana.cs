using System;
using Microsoft.Data.Sqlite;

namespace UserProject
{
    public class SqliteBanana
    {
        public SqliteBanana(string[] args)
        {
            using(var conn = new SqliteConnection("Data Source=:memory:"))
            {
                conn.Open();
                Console.WriteLine("I am a banana that uses sqlite3 version " + conn.ServerVersion);
            }
        }
    }
}