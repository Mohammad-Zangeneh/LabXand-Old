using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LabXand.Logging.Core
{
    [Serializable]
    [DataContract]
    public class ApiLogEntry : IRootLogEntry
    {
        public ApiLogEntry()
        {
            Id = Guid.NewGuid();
            ServiceEntry = new List<ServiceLogEntry>();
            Exceptions = new List<ExceptionInformation>();
        }
        [DataMember]
        public Guid Id { get; protected set; }
        [DataMember]
        public string ControllerName { get; set; }
        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        /// <summary>
        /// The application that made the request.
        /// </summary>

        public string Application { get; set; }
        /// <summary>
        /// The user that made the request.
        /// </summary>
        [DataMember]
        public string User { get; set; }
        /// <summary>
        /// The user that made the request.
        /// </summary>
        [DataMember]
        public string SupplementaryUserInformation { get; set; }
        /// <summary>
        /// The machine that made the request.
        /// </summary>
        [DataMember]
        public string Machine { get; set; }
        /// <summary>
        /// The IP address that made the request.
        /// </summary>
        [DataMember]
        public string RequestIpAddress { get; set; }
        /// <summary>
        /// The request content type.
        /// </summary>
        [DataMember]
        public string RequestContentType { get; set; }
        /// <summary>
        /// The request content body.
        /// </summary>
        [DataMember]
        public string RequestContentBody { get; set; }
        /// <summary>
        /// The request content length.
        /// </summary>
        [DataMember]
        public long RequestContentLength { get; set; }
        /// <summary>
        /// The request URI.
        /// </summary>
        [DataMember]
        public string RequestUri { get; set; }
        /// <summary>
        /// The request method (GET, POST, etc).
        /// </summary>
        [DataMember]
        public string RequestMethod { get; set; }
        /// <summary>
        /// The request route template.
        /// </summary>
        [DataMember]
        public string RequestRouteTemplate { get; set; }
        //public string RequestRouteData { get; set; }        // The request route data.
        /// <summary>
        /// The request headers.
        /// </summary>
        [DataMember]
        public string RequestHeaders { get; set; }
        /// <summary>
        /// The request timestamp.
        /// </summary>
        [DataMember]
        public DateTime? RequestTimestamp { get; set; }
        /// <summary>
        /// The response content type.
        /// </summary>
        [DataMember]
        public string ResponseContentType { get; set; }
        /// <summary>
        /// The response content Length.
        /// </summary>
        [DataMember]
        public long ResponseContentLength { get; set; }
        ///// <summary>
        ///// The response content body.
        ///// </summary>
        //public string ResponseContentBody { get; set; }
        /// <summary>
        /// The response status code.
        /// </summary>
        [DataMember]
        public int? ResponseStatusCode { get; set; }
        /// <summary>
        /// The response headers.
        /// </summary>
        [DataMember]
        public string ResponseHeaders { get; set; }
        /// <summary>
        /// The response timestamp.
        /// </summary>
        [DataMember]
        private DateTime? _ResponseTimestamp;

        [DataMember]
        public DateTime? ResponseTimestamp
        {
            get { return _ResponseTimestamp; }
            set
            {
                _ResponseTimestamp = value;
                if (RequestTimestamp.HasValue)
                    ElapsedTime = value.Value.Subtract(RequestTimestamp.Value).Milliseconds;
            }
        }

        [DataMember]
        public long ElapsedTime { get; private set; }

        [DataMember]
        public IList<ServiceLogEntry> ServiceEntry { get; set; }
        [DataMember]
        public IList<ExceptionInformation> Exceptions { get; set; }

        private string _description;
        [DataMember]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = string.IsNullOrWhiteSpace(value) ? string.Format("Api Log Entry.\n\rCalling '{0}' Api by {1} from {2}", ActionName, User, RequestIpAddress) : value;
            }
        }

        public void SetDetails(ILogEntry detailsEntry)
        {
            ServiceLogEntry serviceLogEntry = detailsEntry as ServiceLogEntry;
            if (serviceLogEntry != null)
                ServiceEntry.Add(serviceLogEntry);
        }

        public void ExceptionOccured(string message, ExceptionInformation exception)
        {
            string description = string.IsNullOrWhiteSpace(Description) ? string.Empty : $"{Environment.NewLine}{Description}";
            Description = $"[{message}]{description}";
            Exceptions.Add(exception);
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            //return base.ToString();
        }

        public void AppendDescription(string description)
        {
            Description += Environment.NewLine + description;
        }
    }
}
