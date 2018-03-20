using SQLite;

namespace XamCnblogs.Portable.Interfaces
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
