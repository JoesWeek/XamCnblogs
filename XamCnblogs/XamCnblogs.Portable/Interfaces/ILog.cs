using System;

namespace XamCnblogs.Portable.Interfaces
{
    public interface ILog
    {
        void SaveLog(string tag, Exception ex);
    }
}
