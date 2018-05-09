using System.Net;

namespace XamCnblogs.Portable.Model
{
    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}
