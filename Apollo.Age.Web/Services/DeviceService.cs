using System.Web;
namespace Apollo.Age.Web.Services
{
    public class DeviceService
    {
        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string ClientIp { get; set; }

        public DeviceService(HttpRequestBase request)
        {
            DomainName = request.ServerVariables["SERVER_NAME"] as string;
            if (request.Browser.IsMobileDevice || request.UserAgent.ToLower().Contains("android"))
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.MajorVersion;
                Platform =request.UserAgent.ToLower().Contains("android") ? "andorid" : request.Browser.MobileDeviceModel;
            }
            else
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.Version;
                Platform = request.Browser.Platform;
            }
           
            CountryCode = string.Empty ;
            ClientIp = request.ServerVariables["REMOTE_ADDR"] as string;                
        }

       
    }
}