using LabXand.Core;
using LabXand.Logging.Core;
using System;
using System.Collections.Generic;

namespace LabXand.Logging.Elasticsearch
{
    public interface IElasticsearchManager
    {
        void InsertIntoElasticSearch(ApiLogEntry entry);
        IList<ApiLogEntryDto> UserActivitySearch(string userName, int from = 0);
        IList<ApiLogEntryDto> ServiceSearch(string serviceName, int from = 0);
        SystemDetails GetDataSubsystem(string organizationFieldName);
        IList<ApiLogEntryDto> Search(SpecificationOfDataList<ApiLogEntryDto> specification, out long total);
        IList<DailyReport> GetDailyInformation(DateTime? start = null, DateTime? end = null);
        IList<DocumentReport> GetBrowserName(DateTime? start, DateTime? end);
        IList<DocumentReport> GetSubSystemFromElasticSearch();
        IList<DocumentReport> GetControllerNameFromElasticSearch();
    }
}
