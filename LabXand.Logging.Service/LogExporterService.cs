using LabXand.Logging.Core;
using LabXand.Logging.Data;
using System.ServiceProcess;
using LabXand.Logging.Elasticsearch;

namespace LabXand.Logging.Service
{
    public partial class LogExporterService : ServiceBase
    {
        public LogExporterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            new FileLogger().Log("LogExporterService Started ... ");
            new LogDataInitializer(new LogRepository(new ElasticsearchManager())).Listen();
        }

        protected override void OnStop()
        {
            new FileLogger().Log("LogExporterService Stoped ... ");
        }
    }
}
