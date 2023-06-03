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

            result.IPv4 = ipAddressv4.ToString();

            result.Hostname = ipAddressv4.ToString();

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
