using LabXand.Logging.Core;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{

    [ElasticsearchType(Name = "DetailsLogEntry")]
    public class DetailsLogEntryDto
    {
        public static DetailsLogEntryDto MapTo(DetailsLogEntry source1)
        {

            if (source1 is DataBaseLogEntry)
            {
                var source = (DataBaseLogEntry)source1;
                var domainDto = new DetailsLogEntryDto();
                domainDto.Id = source.Id;
                domainDto.Title = source.Title;
                domainDto.Type = source.Type;
                domainDto.DomainName = source.DomainName;
                domainDto.IdentityValue = source.IdentityValue;
                domainDto.OperationType = source.OperationType;
                domainDto.Description = source.Description;
                return domainDto;
            }
            else {
                var source = source1;
                var domainDto = new DetailsLogEntryDto();
                domainDto.Id = source.Id;
                domainDto.Title = source.Title;
                domainDto.Type = source.Type;
                //domainDto.DomainName = source.DomainName;
                //domainDto.IdentityValue = source.IdentityValue;
                //domainDto.OperationType = source.OperationType;
                //domainDto.Description = source.Description;
                return domainDto;
            }

        }
        public int Id { get; set; }
        public LogEntryTypes Type { get; set; }
        public string Title { get; set; }
        public string DomainName { get; set; }
        public string IdentityValue { get; set; }
        public OperationTypes OperationType { get; set; }
        public string Description { get; set; }
    }
}
