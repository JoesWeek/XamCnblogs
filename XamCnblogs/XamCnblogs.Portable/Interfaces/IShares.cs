namespace XamCnblogs.Portable.Interfaces
{
    public interface IShares
    {
        void Shares(string url, string title);
        void SharesIcon(string url, string title, object icon);
    }
}
