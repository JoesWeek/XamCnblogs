using SQLite;
using System;
using System.IO;
using XamCnblogs.iOS.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SQLite_iOS))]
namespace XamCnblogs.iOS.Helpers
{
    public class SQLite_iOS : ISQLite
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

                    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                    var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
                    path = Path.Combine(libraryPath, sqliteFilename);
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
