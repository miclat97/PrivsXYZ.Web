using PrivsXYZ.MVC.Models;
using System.Net;

namespace PrivsXYZ.MVC.Services
{
    public class ClientInfoService : IClientInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClientInfoModel GetUserData()
        {
            ClientInfoModel result = new ClientInfoModel();

            var ipAddressv4 = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.MapToIPv4();

            string userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            string operatingSystem = userAgent.Split('(')[1].Split(')')[0];


            result.IPv4 = ipAddressv4.ToString();
            string[] host = Dns.GetHostEntry(ipAddressv4).HostName.Split('.');
            result.Hostname = host[0];
            result.UserAgent = userAgent;
            result.OperatingSystem = operatingSystem;
            result.IPv4 = ipAddressv4.ToString();
            result.Hostname = host[1];

            try
            {
                var hostEntry = Dns.GetHostEntry(result.IPv4)!.HostName;
                if (hostEntry != null)
                {
                    result.Hostname = hostEntry;
                }
            }
            catch
            {

            }

            return result;
        }
    }
}
