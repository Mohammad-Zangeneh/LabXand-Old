using LabXand.Logging.Core;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.Elasticsearch
{
    [ElasticsearchType(Name = "ExceptionInformation")]
    public class ExceptionInformationDto
    {
        public long Id { get; set; }
        public Guid ParentId { get; set; }
        //public ApiLogEntry Parent { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Data { get; set; }
        public static ExceptionInformationDto MapTo(ExceptionInformation source)
        {
            var domainDto = new ExceptionInformationDto();
            domainDto.Data = source.Data;
            domainDto.Id = source.Id;
            domainDto.Message = source.Message;
            domainDto.ParentId = source.ParentId;
            domainDto.StackTrace = source.StackTrace;

            return domainDto;
        }
    }
}
