using LabXand.Core.Messaging.MSMQ;
using System;

namespace LabXand.Logging.Core
{
    public class MSMQLogger : ILogger
    {        
        public int Log(ApiLogEntry logEntry)
        {
            try
            {
                MSMQHelper.SendMessage(logEntry.Description, logEntry, @".\Private$\labXand.logging.repository");
               // new FileLogger().Log(logEntry);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
