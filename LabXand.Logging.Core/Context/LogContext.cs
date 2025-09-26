using System.Net.Http;
using System.Web;

namespace LabXand.Logging.Core
{
    public class EmptyLogContext : ILogContext
    {
        public void AppendDescription(string message)
        {
            
        }
    }

    public class LogContext<TEntry> : ILogContext<TEntry>
        where TEntry : class, IRootLogEntry
    {
        const string KEY = "ApiLogEntry";
        public TEntry Current
        {
            get
            {
                var httpRequest = Get();
                if (httpRequest == null)
                    return null;
                if (!httpRequest.Properties.ContainsKey(KEY))
                    return null;

                return httpRequest.Properties[KEY] as TEntry;
            }
        }

        public void AppendDescription(string message)
        {
            if (Current != null)
                Current.AppendDescription(message);
        }

        public void InitiateEntry(TEntry entry)
        {
            Get().Properties.Add(KEY, entry);
        }

        HttpRequestMessage Get()
        {
            const string key = "MS_HttpRequestMessage";
            if (HttpContext.Current == null)
                return null;
            if (!HttpContext.Current.Items.Contains(key))
                return null;
            return HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
        }
    }
}
