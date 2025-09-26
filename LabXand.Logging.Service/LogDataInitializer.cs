using LabXand.Core.ExceptionManagement;
using LabXand.Logging.Core;
using LabXand.Logging.Data;

namespace LabXand.Logging.Service
{
    public class LogDataInitializer
    {
        private System.Messaging.MessageQueue _messageQueue;
        ILogRepository _logRepository;

        public LogDataInitializer(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        private const string DefaultPath = @".\Private$\labXand.logging.repository";

        public void Listen()
        {            
            _messageQueue = new System.Messaging.MessageQueue(DefaultPath);
            _messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            _messageQueue.Formatter = new System.Messaging.BinaryMessageFormatter();
            _messageQueue.BeginReceive();
        }

        private void MessageQueue_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
            string messageLabel = null;
            IRootLogEntry messageBody = null;
            try
            {
                new FileLogger().Log("Recived message form MSMQ ....");
                System.Messaging.Message Message = _messageQueue.EndReceive(e.AsyncResult);
                Message.Formatter = new System.Messaging.BinaryMessageFormatter();

                messageLabel = Message.Label;
                messageBody = (IRootLogEntry)Message.Body;

                ProcessMessage(messageBody);
            }
            catch (System.Exception ex)
            {
                new FileLogger().Log(ExceptionHelper.GetDetails(ex));
            }

            _messageQueue.BeginReceive();
        }

        private void ProcessMessage(IRootLogEntry messageBody)
        {
            _logRepository.AddEntry(messageBody);
        }
    }
}
