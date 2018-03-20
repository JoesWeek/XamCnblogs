using SQLite;
using System;
using System.IO;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SQLite_Android))]
namespace XamCnblogs.Droid.Helpers
{
    public class SQLite_Android : ISQLite
    {
        private static string GetDatabasePath()
        {
            const string sqliteFilename = "xamcnblogs.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            return path;
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var dbPath = GetDatabasePath();

            // Return the synchronous database connection 
            return new SQLiteAsyncConnection(dbPath);
        }
    }
}