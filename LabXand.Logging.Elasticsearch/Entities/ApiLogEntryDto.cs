using LabXand.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace LabXand.Logging.Elasticsearch
{
    [ElasticsearchType(Name = "ApiLogEntry")]
    public class ApiLogEntryDto
    {
        static string getOs(string userAgent)
        {
            if (userAgent == null)
                return "Unknown";
            // Match user agent string with operating systems 
            if (userAgent.IndexOf("Windows 95") != -1 || userAgent.IndexOf("Windows_95") != -1 || userAgent.IndexOf("Win95") != -1)
                return "Windows 95";
            if (userAgent.IndexOf("Android") != -1)
                return "Android";
            if (userAgent.IndexOf("Windows 98") != -1 || userAgent.IndexOf("Win98") != -1)
                return "Windows 98";
            if (userAgent.IndexOf("Windows NT 5.0") != -1 || userAgent.IndexOf("Windows 2000") != -1)
                return "Windows 2000";
            if (userAgent.IndexOf("Windows NT 5.1") != -1 || userAgent.IndexOf("Windows XP") != -1)
                return "Windows XP";
            if (userAgent.IndexOf("Windows NT 5.2") != -1)
                return "Windows Server 2003";
            if (userAgent.IndexOf("Windows NT 6.0") != -1)
                return "Windows Vista";
            if (userAgent.IndexOf("Windows NT 6.1") != -1)
                return "Windows 7";
            if (userAgent.IndexOf("Windows NT 6.2") != -1)
                return "Windows 8";
            if (userAgent.IndexOf("Windows NT 10.0") != -1)
                return "Windows 10";
            if (userAgent.IndexOf("Windows NT 4.0") != -1 || userAgent.IndexOf("WinNT4.0") != -1 || userAgent.IndexOf("WinNT") != -1
                || userAgent.IndexOf("Windows NT") != -1)
                return "Windows NT 4.0";
            if (userAgent.IndexOf("Windows ME") != -1)
                return "Windows ME";
            if (userAgent.IndexOf("OpenBSD") != -1)
                return "Open BSD";
            if (userAgent.IndexOf("SunOS") != -1)
                return "Sun OS";
            if (userAgent.IndexOf("Linux") != -1 || userAgent.IndexOf("X11") != -1)
                return "Linux";
            if (userAgent.IndexOf("Mac_PowerPC") != -1 || userAgent.IndexOf("Macintosh") != -1)
                return "Mac OS";
            if (userAgent.IndexOf("QNX") != -1)
                return "QNX";

            return "Unknown";
        }
        private static string GetBrowserName(string userAgent)
        {
            //            start at the root (Default node)
            //if (user agent contains Opera) return Opera;
            //else if (user agent contains Mozilla) then
            //                if (user agent contains AppleWebKit) then
            //                               if (user agent contains Chrome) return Chrome;
            //                               else if (user agent contains Safari) return Safari;
            //                               else return WebKit;
            //                else if (user agent contains Firefox) return Firefox;
            //                else if (user agent contains Trident) return IE;
            //                else return Mozilla;
            //else return Unknown;
            if (userAgent == null)
                return "Unknown";
            if (userAgent.IndexOf("Opera") != -1)
                return "Opera";
            else if (userAgent.IndexOf("Mozilla") != -1)
            {

                if (userAgent.IndexOf("AppleWebKit") != -1)
                {
                    if (userAgent.IndexOf("Edge") != -1)
                        return "Edge";
                    if (userAgent.IndexOf("Chrome") != -1)
                        return "Chrome";
                    if (userAgent.IndexOf("Safari") != -1)
                        return "Safari";
                    if (userAgent.IndexOf("WebKit") != -1)
                        return "WebKit";
                }

                else if (userAgent.IndexOf("Firefox") != -1)
                    return "Firefox";
                else if (userAgent.IndexOf("Trident") != -1)
                    return "IE";
                else
                    return "Mozilla";
            }
            return "Unknown";
        }
        static string getVersion(string browserName, string userAgent)
        {
            if (browserName == "Unknown")
                return "0";
            var index = userAgent.IndexOf(browserName);
            if (index != -1)
            {
                var version = userAgent.Substring(index + browserName.Length + 1, 4);
                return version;
            }
            return "0";
        }
        public static ApiLogEntryDto Mapto(ApiLogEntry source)
        {
            try
            {
                var domainDto = new ApiLogEntryDto();
                domainDto.ActionName = source.ActionName;
                domainDto.Application = string.IsNullOrWhiteSpace(source.Application) ? "WithoutApplicationName" : source.Application;
                domainDto.ControllerName = source.ControllerName;
                domainDto.ElapsedTime = source.ElapsedTime;
                domainDto.Exceptions = source.Exceptions != null ? source.Exceptions.Select(t => ExceptionInformationDto.MapTo(t)).ToList() : new List<ExceptionInformationDto>();
                domainDto.Id = source.Id;
                domainDto.Machine = source.Machine;
                domainDto.RequestContentBody = source.RequestContentBody;
                domainDto.RequestContentLength = source.RequestContentLength;
                domainDto.RequestContentType = source.RequestContentType;
                domainDto.RequestHeaders = source.RequestHeaders;
                domainDto.RequestIpAddress = source.RequestIpAddress;
                domainDto.RequestMethod = source.RequestMethod;
                domainDto.RequestRouteTemplate = source.RequestRouteTemplate;
                domainDto.RequestTimestamp = source.RequestTimestamp;
                domainDto.RequestUri = source.RequestUri;
                domainDto.ResponseContentLength = source.ResponseContentLength;
                domainDto.ResponseContentType = source.ResponseContentType;
                domainDto.ResponseHeaders = source.ResponseHeaders;
                domainDto.ResponseStatusCode = source.ResponseStatusCode;
                domainDto.ResponseTimestamp = source.ResponseTimestamp;
                domainDto.Description = source.Description;
                domainDto.ServiceEntry = source.ServiceEntry != null ? source.ServiceEntry.Select(t => ServiceLogEntryDto.MapTo(t)).ToList() : new List<ServiceLogEntryDto>();

                domainDto.User = source.User;
                domainDto.BrowserName = GetBrowserName(source.RequestHeaders);
                domainDto.BrowserVersion = getVersion(domainDto.BrowserName, source.RequestHeaders);
                domainDto.Os = getOs(source.RequestHeaders);
                domainDto.Lenght = source.RequestContentLength + source.ResponseContentLength;
                return domainDto;
            }
            catch (Exception ex)
            {
                new FileLogger().Log(source);
                throw ex;
            }
        }
        //===================================
        public Guid Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Application { get; set; }
        public string User { get; set; }
        public object SupplementaryUserInformation { get; set; }
        public string Machine { get; set; }
        public string RequestIpAddress { get; set; }
        public string RequestContentType { get; set; }
        public string RequestContentBody { get; set; }
        public long RequestContentLength { get; set; }
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public string RequestRouteTemplate { get; set; }
        public string RequestHeaders { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string ResponseContentType { get; set; }
        public long ResponseContentLength { get; set; }
        public int? ResponseStatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime? ResponseTimestamp { set; get; }
        public string Description { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string Os { get; set; }
        public long Lenght { get; set; }
        public long ElapsedTime { get; set; }
        [Nested]
        public IList<ServiceLogEntryDto> ServiceEntry { get; set; }
        [Nested]
        public IList<ExceptionInformationDto> Exceptions { get; set; }
    }
}
