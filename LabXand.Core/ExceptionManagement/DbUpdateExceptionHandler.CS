using Newtonsoft.Json;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Text;

namespace LabXand.Core.ExceptionManagement
{
    public class DbUpdateExceptionHandler : CustomExceptionHandlerBase<DbUpdateException>
    {
        public override string GetUserMessage(Exception exception) => "به‌روزرسانی داده‌ها در پایگاه داده با مشکل مواجه شد";

        public override string GetTechnicalDetails(Exception exception)
        {
            DbUpdateException dbUpdateException = exception as DbUpdateException;
            StringBuilder messageStringBuilder = new StringBuilder();
            StringBuilder stackTraceStringBuilder = new StringBuilder();
            messageStringBuilder.AppendLine("An error occurred while updating the database.");
            stackTraceStringBuilder.AppendLine(dbUpdateException.StackTrace);
            if (exception.InnerException != null)
            {
                messageStringBuilder.AppendLine($"Inner exception details: {dbUpdateException.InnerException.Message}");
                stackTraceStringBuilder.AppendLine(exception.InnerException.StackTrace);
            }

            foreach (var entry in dbUpdateException.Entries)
            {
                messageStringBuilder.AppendLine($"Entity of type {entry.Entity.GetType().Name} in state {entry.State} caused an exception.");
                var entity = entry.Entity;
                messageStringBuilder.AppendLine($"Entity details: {entity}");
                var entityDetails = JsonConvert.SerializeObject(entry.Entity, Formatting.Indented);
                messageStringBuilder.AppendLine($"Entity details: {entityDetails}");
            }

            if (dbUpdateException.InnerException is SqlException sqlEx)
            {
                foreach (SqlError error in sqlEx.Errors)
                {
                    messageStringBuilder.AppendLine($"SQL Error: Number {error.Number}, Message: {error.Message}, LineNumber: {error.LineNumber}, Source: {error.Source}, Procedure: {error.Procedure}");
                }

                switch (sqlEx.Number)
                {
                    case 2627: 
                        messageStringBuilder.AppendLine("A unique constraint was violated.");
                        break;
                    case 547:
                        messageStringBuilder.AppendLine("A constraint check was violated.");
                        break;
                    case 2601:
                        messageStringBuilder.AppendLine("A duplicate key was inserted.");
                        break;
                    default:
                        messageStringBuilder.AppendLine("A database error occurred. Please try again.");
                        break;
                }
            }
            messageStringBuilder.AppendLine(stackTraceStringBuilder.ToString());
            return messageStringBuilder.ToString();
        }
    }
}