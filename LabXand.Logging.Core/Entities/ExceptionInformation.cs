using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace LabXand.Logging.Core
{
    public interface ILogException
    { }
    [Serializable]
    [DataContract]

    public class ExceptionInformation : ILogEntry, ILogException
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public Guid ParentId { get; set; }
       // [DataMember]
        public ApiLogEntry Parent { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public string Data { get; set; }
        [DataMember]
        public string Description
        {
            get
            {
                return string.Format("Exception Log Entry.\n\rException Message is : {0}", this.Message);
            }
        }

        public static ExceptionInformation CreateFromException(Guid parentEntryId, Exception exception)
        {
            ExceptionInformation exceptionInformation = new ExceptionInformation()
            {
                ParentId = parentEntryId
            };
            StringBuilder messageStringBuilder = new StringBuilder();
            StringBuilder stackTraceStringBuilder = new StringBuilder();
            StringBuilder dataStringBuilder = new StringBuilder();
            int level = 1;
            while (exception != null)
            {
                messageStringBuilder.AppendLine(string.Format("{0}. {1}", level, exception.Message));
                stackTraceStringBuilder.AppendLine(string.Format("{0}. {1}", level, exception.StackTrace));
                if (exception.Data != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (DictionaryEntry item in exception.Data)
                    {
                        stringBuilder.AppendLine(string.Format("[{0}] : {1}", item.Key, item.Value));
                    }
                    dataStringBuilder.AppendLine(string.Format("{0}. {1}", level, stringBuilder.ToString()));
                }
                level++;
                exception = exception.InnerException;
            }

            exceptionInformation.Message = messageStringBuilder.ToString();
            exceptionInformation.StackTrace = stackTraceStringBuilder.ToString();
            exceptionInformation.Data = dataStringBuilder.ToString();

            return exceptionInformation;
        }
    }
}
