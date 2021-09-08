using SQLite;
using System;

namespace Desktop.GrupoBW.DAL
{
    public class Database
    {
        protected readonly SQLiteAsyncConnection _database;
        string dbCaminho = AppDomain.CurrentDomain.BaseDirectory + "database.db3";

        public Database()
        {
            _database = new SQLiteAsyncConnection(dbCaminho);
        }
    }
}
