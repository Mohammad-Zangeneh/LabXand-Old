using LabXand.Logging.Core;
using LabXand.Logging.Elasticsearch;

namespace LabXand.Logging.Data
{
    public interface ILogRepository
    {
        void AddEntry(IRootLogEntry entry);
    }

    public class LogRepository : ILogRepository
    {
        private readonly IElasticsearchManager elasticsearchManager;

        public LogRepository(IElasticsearchManager elasticsearchManager)
        {
            this.elasticsearchManager = elasticsearchManager;
        }

        public void AddEntry(IRootLogEntry entry)
        {
            //add elastic search there

            //_dbContext.Add((ApiLogEntry)entry);
            //_dbContext.Commit();
            //new FileLogger().Log("from elastic");
            //new FileLogger().Log((ApiLogEntry)entry);
            //new FileLogger().Log("end from elastic");

            elasticsearchManager.InsertIntoElasticSearch((ApiLogEntry)entry);
        }

    }




}
