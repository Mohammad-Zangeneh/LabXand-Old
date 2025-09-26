using LabXand.Core.Messaging.MSMQ;
using LabXand.Logging.Core;
using Newtonsoft.Json;
using System;
using System.Web.Configuration;

namespace LabXand.Logging.LogService
{
    public class ElasticLogger<TUserContext> : ILogger
    {
        public ElasticLogger()
        {
            var url = WebConfigurationManager.AppSettings["ElUrl"];

            var username = WebConfigurationManager.AppSettings["ELUserName"] ?? "labxand";

            var password = WebConfigurationManager.AppSettings["ELPassword"] ?? "LabXanDMorsa725";
        }
        public int Log(ApiLogEntry logEntry)
        {
            try
            {
                var title = logEntry.Description?.Length > 200 ? logEntry.Description.Substring(0, 200) : logEntry.Description  ?? "UnknownTitle";
                MSMQHelper.SendMessage(title, logEntry, @".\Private$\labXand.logging.repository");
                return 1;
            }
            catch (Exception)
            {
                var df = new DefaultLogPathCreator();
                var logPath = WebConfigurationManager.AppSettings["ElasticSearchLogPath"] ?? df.Directory;
                new FileLogger().Log(logPath, Guid.NewGuid().ToString(), "txt", JsonConvert.SerializeObject(logEntry));
                return -1;
            }
        }
    }
}