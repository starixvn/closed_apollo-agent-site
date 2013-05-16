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
            if (request.Browser.IsMobileDevice)
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.MajorVersion;
                Platform = request.Browser.MobileDeviceModel;
            }
            else
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.Version;
                Platform = GetPlatform(request);
            }
           
            CountryCode = string.Empty ;
            ClientIp = request.ServerVariables["REMOTE_ADDR"] as string;                
        }

        private static string GetPlatform(HttpRequestBase request)
        {
            // Create a list of all platform
            string[] platform = new[] { "Windows 3", "Win16", "Windows 95", "Win95", "Windows 98", "Win98", "Windows 2000", "Windows XP", "Windows Server 2003", "Windows Vista", "Windows NT", "WinNT", "Windows ME", "Open BSD", "OpenBSD", "Sun OS", "SunOS", "Linux", "Mac OS", "Mac_PowerPC", "Macintosh", "QNX", "QNX", "BeOS", "BeOS", "OS/2", "OS/2" };
            

            // Loop through each item in the list created above and check if the header contains that text
            foreach (string s in platform)
            {
                if (request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                {
                    return s;
                }
            }

            return "Unknown";
        }
    }
}