using LabXand.Logging.Core;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
    [ElasticsearchType(Name = "ServiceLogEntry")]
    public class ServiceLogEntryDto
    {
        public static ServiceLogEntryDto MapTo(ServiceLogEntry source)
        {
            var domainDto = new ServiceLogEntryDto();
            domainDto.CompleteTime = source.CompleteTime;
            domainDto.Description = source.Description;
            domainDto.Details = source.Details != null ? source.Details.Select(t => DetailsLogEntryDto.MapTo(t)).ToList() : new List<DetailsLogEntryDto>();
            domainDto.ElapsedTime = source.ElapsedTime;
            domainDto.Id = Guid.NewGuid();
            domainDto.OperationName = source.OperationName;
            domainDto.ServiceName = source.ServiceName;
            domainDto.StartTime = source.StartTime;
            domainDto.Status = source.Status;
            domainDto.UnitOfWorkInstanceId = source.UnitOfWorkInstanceId;
            domainDto.Type = source.Type;

            return domainDto;
        }
        public Guid Id { get; set; }
        public Guid UnitOfWorkInstanceId { get; set; }
        public ServiceOperationStatus Status { get; set; }
        public ServiceOperationTypes Type { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string OperationName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public long ElapsedTime { get; set; }
        [Nested]
        public List<DetailsLogEntryDto> Details { get; set; }
    }
}
