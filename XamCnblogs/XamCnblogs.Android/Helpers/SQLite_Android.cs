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
        private static string path;

        private static SQLiteAsyncConnection connectionAsync;

        private static readonly object locker = new object();
        private static readonly object pathLocker = new object();

        private static string GetDatabasePath()
        {
            lock (pathLocker)
            {
                if (path == null)
                {
                    const string sqliteFilename = "xamcnblogs.db3";
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Documents folder
                    path = Path.Combine(documentsPath, sqliteFilename);
                }
            }
            return path;
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            lock (locker)
            {
                if (connectionAsync == null)
                {
                    var dbPath = GetDatabasePath();
                    connectionAsync = new SQLiteAsyncConnection(dbPath);
                }
            }
            return connectionAsync;
        }
    }
}