using LabXand.Core;
using LabXand.Logging.Core;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.Logging.Elasticsearch
{
    public class ElasticsearchManager : IElasticsearchManager
    {
        protected Uri node;
        protected ConnectionSettings settings;
        protected ElasticClient client;
        protected string _defaultIndex;
        public ElasticsearchManager(string elUserName = "labxand", string elPassword = "LabXanDMorsa725", string elUrl = "localhost:9201", string defaultIndex = "labxand_logger3")
        {

            _defaultIndex = defaultIndex;
            string url = string.Concat("http://", elUserName, ":", elPassword, "@", elUrl);
            node = new Uri(url);
            settings = new ConnectionSettings(node).DefaultIndex(_defaultIndex);
            client = new ElasticClient(settings);
        }
        public void InsertIntoElasticSearch(ApiLogEntry entry)
        {
            if (entry.Application == "Logging")
                return;
            if (!client.Indices.Exists(_defaultIndex).Exists)
                client.Indices.Create(_defaultIndex, index => index.Map<ApiLogEntryDto>(x => x.AutoMap()));
            var enteryDto = ApiLogEntryDto.Mapto(entry);

            var indexResponse = client.IndexDocument(enteryDto);
            if (!indexResponse.IsValid)
                throw indexResponse.OriginalException;
        }
        private string ConvertToCamelCase(string str)
        {
            var r = str.Split('.');
            var result = Char.ToLowerInvariant(r[0][0]) + r[0].Substring(1);
            foreach (var item in r.Skip(1))
            {
                result += "." + Char.ToLowerInvariant(item[0]) + item.Substring(1);
            }
            return result;

        }
        public IList<ApiLogEntryDto> Search(SpecificationOfDataList<ApiLogEntryDto> specification, out long total)
        {
            // GetResponseStatusInformation();
            SearchDescriptor<ApiLogEntryDto> se = new SearchDescriptor<ApiLogEntryDto>();
            se.From(specification.PageIndex * specification.PageSize);

            SortDescriptor<ApiLogEntryDto> sort = new SortDescriptor<ApiLogEntryDto>();
            if (specification.SortSpecifications != null && specification.SortSpecifications.Count > 0)
            {
                specification.SortSpecifications = specification.SortSpecifications.Where(t => t.SortField != "Id").ToList();
            }
            if (specification.SortSpecifications != null && specification.SortSpecifications.Count > 0)
            {
                var sortType = new ApiLogEntryDto().GetType().GetProperty(specification.SortSpecifications[0].SortField);
                if (sortType != null && sortType.PropertyType.Name == "String")
                    specification.SortSpecifications[0].SortField = specification.SortSpecifications[0].SortField + ".keyword";
                if (specification.SortSpecifications[0].AscendingSortDirection)
                    sort.Ascending(ConvertToCamelCase(specification.SortSpecifications[0].SortField));
                else
                    sort.Descending(ConvertToCamelCase(specification.SortSpecifications[0].SortField));

                if (sortType != null)
                    se.Sort(s => sort);
            }
            else
                se.Sort(s => s.Descending(t => t.RequestTimestamp));

            se.Take(specification.PageSize);
            QueryContainer mainQuery = null;

            foreach (var item in specification.FilterSpecifications)
            {
                if (item.FilterOperation == FilterOperations.GreaterThanOrEqual)
                {
                    var sortType = new ApiLogEntryDto().GetType().GetProperty(item.PropertyName);
                    if (sortType != null && sortType.PropertyType.FullName.IndexOf("DateTime") != -1)
                    {
                        var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(v => v.Field(ConvertToCamelCase(item.PropertyName)).GreaterThanOrEquals(Convert.ToDateTime(item.FilterValue).ToUniversalTime()));
                        if (mainQuery == null)
                            mainQuery = dateRangeForDateTime;
                        else
                            mainQuery = mainQuery && dateRangeForDateTime;
                    }
                }
                else if (item.FilterOperation == FilterOperations.GreaterThan)
                {
                    var sortType = new ApiLogEntryDto().GetType().GetProperty(item.PropertyName);
                    if (sortType != null && sortType.PropertyType.FullName.IndexOf("DateTime") != -1)
                    {
                        var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(v => v.Field(ConvertToCamelCase(item.PropertyName)).GreaterThan(Convert.ToDateTime(item.FilterValue).ToUniversalTime()));
                        if (mainQuery == null)
                            mainQuery = dateRangeForDateTime;
                        else
                            mainQuery = mainQuery && dateRangeForDateTime;
                    }
                }
                else if (item.FilterOperation == FilterOperations.LessThan)
                {
                    var sortType = new ApiLogEntryDto().GetType().GetProperty(item.PropertyName);
                    if (sortType != null && sortType.PropertyType.FullName.IndexOf("DateTime") != -1)
                    {
                        var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(v => v.Field(ConvertToCamelCase(item.PropertyName)).LessThan(Convert.ToDateTime(item.FilterValue).ToUniversalTime()));
                        if (mainQuery == null)
                            mainQuery = dateRangeForDateTime;
                        else
                            mainQuery = mainQuery && dateRangeForDateTime;
                    }
                }
                else if (item.FilterOperation == FilterOperations.LessThanOrEqual)
                {
                    var sortType = new ApiLogEntryDto().GetType().GetProperty(item.PropertyName);
                    if (sortType != null && sortType.PropertyType.FullName.IndexOf("DateTime") != -1)
                    {

                        var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().
                            DateRange(v => v.Field(ConvertToCamelCase(item.PropertyName)).LessThanOrEquals(Convert.ToDateTime(item.FilterValue).ToUniversalTime()));
                        if (mainQuery == null)
                            mainQuery = dateRangeForDateTime;
                        else
                            mainQuery = mainQuery && dateRangeForDateTime;
                    }
                }
                else if (item.FilterOperation == FilterOperations.Equal)
                {

                    QueryContainerDescriptor<ApiLogEntryDto> tt = new QueryContainerDescriptor<ApiLogEntryDto>();

                    if (item.PropertyName.IndexOf(".First()") != -1)
                    {
                        var field = ConvertToCamelCase(item.PropertyName);
                        field = field.Replace(".first()", "");
                        var paths = field.Split('.');
                        var path = paths[0];
                        if (paths.Length > 2)
                            path += "." + paths[1];
                        tt.Nested(n => n.Path(path)
                                    .Query(q => q.Term(field + ".keyword", item.FilterValue))
                        );
                    }
                    else if (item.PropertyName.IndexOf(".Has()") != -1)
                    {
                        if (item.FilterValue.ToString() == "True")
                        {
                            var field = ConvertToCamelCase(item.PropertyName);
                            field = field.Replace(".has()", "");
                            var paths = field.Split('.');
                            var path = paths[0];
                            if (paths.Length > 2)
                                path += "." + paths[1];
                            tt.Nested(n => n.Path(path).Query(q => q.Bool(b => b.Must(MultiBucketAggregate => MultiBucketAggregate.Exists(ex => ex.Field(path))))));

                        }
                    }
                    else
                        tt.Match(m => m.Field(ConvertToCamelCase(item.PropertyName)).Query(item.FilterValue.ToString()));

                    if (mainQuery == null)
                        mainQuery = tt;
                    else
                        mainQuery = mainQuery && tt;
                }
            }

            se.Query(r => mainQuery);
            var search = client.Search<ApiLogEntryDto>(se);

            total = search.Total;
            //if (total > 5000)
            //    total = 5000;
            return search.Documents.ToList();
        }
        public IList<ApiLogEntryDto> UserActivitySearch(string userName, int from = 0)
        {
            var search = client.Search<ApiLogEntryDto>(s => s
            .From(from)
                   .Query(q => q
                       .Match(m => m
                           .Field("supplementaryUserInformation.userName")
                           .Query(userName)
                       )
                   )

             .Sort(so => so.Descending(t => t.RequestTimestamp))
             .Source(so => so.IncludeAll())
           );
            //Console.WriteLine("Application  - ControllerName - ActionName - date");
            //foreach (var item in search.Documents)
            //{
            //    Console.WriteLine(item.Application + " - " + item.ControllerName + " - " + item.ActionName + " - " + item.RequestTimestamp);
            //}
            //Console.WriteLine(Environment.NewLine + "press space for next page or Escape for back");
            //var k = Console.ReadKey();
            //if (k.Key == ConsoleKey.Spacebar)
            //    userActivitySearch(userName, from + 10);
            return search.Documents.ToList();
        }

        public IList<ApiLogEntryDto> ServiceSearch(string serviceName, int from = 0)
        {

            var searchResults = client.Search<ApiLogEntryDto>(s => s
                    .From(from)
                    .Size(10)
                        .Query(q => q
                        .Nested(n => n
                            .Path(p => p.ServiceEntry)
                            .Query(qq => qq
                                    .Term(t => t.ServiceEntry.First().ServiceName.Suffix("keyword"), serviceName)
                                )
                            )
                        )
                     );

            return searchResults.Documents.ToList();
        }

        private List<SubsystemDetails> getDataGroupByOrganization(string organizationfield)
        {
            var result = new List<SubsystemDetails>();

            var searchResponseError =
                client.Search<ApiLogEntryDto>
                (s => s.Query(q => q.Nested(n => n
                                                   .Path(p => p.Exceptions)
                                                   .Query(qq => qq.Bool(b =>
                                                                           b.Must(mu =>
                                                                               mu.Exists(ex => ex.Field(ff => ff.Exceptions))))
                                                          )
                                               )
                             )

                                                        .Aggregations(a => a
                                                            .Terms("organization", ta => ta
                                                                .Field(organizationfield + ".keyword")
                                                                .Size(40)
                                                         //=======


                                                         )
                                                    )
                                                    .Size(0)
                                               )//end search
                                               ;


            var searchResponse23 = client.Search<ApiLogEntryDto>(s => s
             .Size(0)
             .Aggregations(a => a
                 .Terms("organization", ta => ta
                     .Field(organizationfield + ".keyword")
                     .Size(10)
                 //=======
                 .Aggregations(aa => aa
                  .Sum("sumRequest", m => m
                                    .Field(o => o.RequestContentLength)
                  )
                   .Sum("sumResponse", sa => sa
                         .Field(p => p.ResponseContentLength)
                      )

                    //sum ElapsedTime
                    .Sum("sumOfElapsedTime", et =>
                                         et.Field(t => t.ElapsedTime)
                         )

              )
                 )

              ));

            var termsAggregation2 = searchResponse23.Aggregations.Terms("organization").Buckets.ToList();
            foreach (var item in termsAggregation2)
            {
                var detail = new SubsystemDetails();
                detail.SumOfRequest = item.ValueCount("sumRequest").Value;
                detail.SumOfResponse = item.ValueCount("sumResponse").Value;
                detail.SumOfElapsedTime = item.ValueCount("sumOfElapsedTime").Value;
                detail.Key = item.Key;
                detail.DocumentCount = item.DocCount.Value;
                result.Add(detail);
            }

            var termsAggregationError = searchResponseError.Aggregations.Terms("organization")?.Buckets?.ToList();

            foreach (var item in termsAggregationError)
            {
                var subSystemDetail = result.FirstOrDefault(t => t.Key == item.Key);
                if (subSystemDetail == null)
                {
                    subSystemDetail = new SubsystemDetails
                    {
                        Key = item.Key,
                        ErrorCount = item.DocCount.Value
                    };
                    result.Add(subSystemDetail);
                }
                else
                    subSystemDetail.ErrorCount = item.DocCount.Value;

            }

            return result;
        }
        public SystemDetails GetDataSubsystem(string organizationFieldName = "supplementaryUserInformation.lastName")
        {
            var result = new SystemDetails();
            result.TotalDocument = client.Count<ApiLogEntryDto>().Count;

            var searchResponseError = client.Search<ApiLogEntryDto>(s => s
          .Query(q => q
           .Nested(n => n
                            .Path(p => p.Exceptions)
                            .Query(qq => qq.Bool(b => b.Must(mu => mu.Exists(ex => ex.Field(ff => ff.Exceptions))))

                                )
                            )
          )

           .Aggregations(a => a
               .Terms("application", ta => ta
                   .Field("application.keyword")
                   .Size(10)

                    //=======
                    )
               )
          );//end search


            var searchResponse23 = client.Search<ApiLogEntryDto>(s => s
              .Query(q => q.Bool(b => b.Must(mu => mu.Exists(ex => ex.Field(ff => ff.Application)))))
             .Aggregations(a => a
                 .Terms("application", ta => ta
                     .Field("application.keyword")
                     .Size(10)
                 //=======
                 .Aggregations(aa => aa
                  .Sum("sumRequest", m => m
                                    .Field(o => o.RequestContentLength)
                  )
                   .Sum("sumResponse", sa => sa
                         .Field(p => p.ResponseContentLength)
                      )

              )
                 )

          .Sum("sum_request", sa => sa
              .Field(p => p.RequestContentLength)
          )

          .Sum("sum_response", sa => sa
              .Field(p => p.ResponseContentLength)
          )
          .Sum("sum_elapsedTime", sa => sa
              .Field(p => p.ElapsedTime)
          )
              ));

            //   var termsAggregation = searchResponse.Aggregations.Terms("last_names");

            var termsAggregation2 = searchResponse23.Aggregations.Terms("application").Buckets.ToList();
            //  Console.WriteLine(Environment.NewLine + "log with subsystem");
            foreach (var item in termsAggregation2)
            {
                var detail = new SubsystemDetails
                {
                    SumOfRequest = item.ValueCount("sumRequest").Value,
                    SumOfResponse = item.ValueCount("sumResponse").Value,
                    //detail.SumOfElapsedTime = item.ValueCount("sumElapsedTime").Value;
                    Key = item.Key,
                    DocumentCount = item.DocCount.Value
                };
                result.SubsystemDetails.Add(detail);
                //  Console.WriteLine(item.Key + " = " + item.DocCount + " _ sum request = " + sumRequest + " _ sum response = " + sumResponse);
            }

            var termsAggregationError = searchResponseError.Aggregations.Terms("application")?.Buckets?.ToList();
            result.TotalError = 0;
            foreach (var item in termsAggregationError)
            {
                var subSystemDetail = result.SubsystemDetails.FirstOrDefault(t => t.Key == item.Key);
                if (subSystemDetail == null)
                {
                    subSystemDetail = new SubsystemDetails
                    {
                        Key = item.Key,
                        ErrorCount = item.DocCount.Value
                    };
                    result.SubsystemDetails.Add(subSystemDetail);
                }
                else
                    subSystemDetail.ErrorCount = item.DocCount.Value;
                result.TotalError += subSystemDetail.ErrorCount;
            }

            result.TotalRequestLength = searchResponse23.Aggregations.Sum("sum_request").Value;
            //  Console.WriteLine(Environment.NewLine + "requestLength = " + requestLength.Value);
            result.TotalResponsetLength = searchResponse23.Aggregations.Sum("sum_response").Value;
            result.TotalElapsedTime = searchResponse23.Aggregations.Sum("sum_elapsedTime").Value;
            // Console.WriteLine(Environment.NewLine + "responseLength = " + responseLength.Value);
            result.BrowserDetails = GetBrowserName(null, null);
            result.OrganizationDetails = getDataGroupByOrganization(organizationFieldName);
            result.DialyDetails = GetDailyInformation();
            result.StatusCodeDetails = GetResponseStatusInformation(null, null);
            return result;

        }
        IList<DocumentReport> GetDailyError(DateTime? start, DateTime? end)
        {
            var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                   v => v.Field(ConvertToCamelCase("requestTimestamp"))
               .GreaterThanOrEquals(start.Value.ToUniversalTime()));

            dateRangeForDateTime = dateRangeForDateTime && new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                v => v.Field(ConvertToCamelCase("requestTimestamp"))
            .LessThanOrEquals(end.Value.ToUniversalTime()));

            var searchResponse23 =
                client.Search<ApiLogEntryDto>(s => s
                     .Query(t => dateRangeForDateTime &&
                                t.Nested(n => n.Path(p => p.Exceptions).Query(qq => qq.Bool(b => b.Must(mu => mu.Exists(ex => ex.Field(exf => exf.Exceptions)))))))
                                 .Aggregations(a => a
                                 .DateHistogram("dialy", t => t.Field("requestTimestamp").Format("HH").TimeZone("Asia/Tehran").Interval(DateInterval.Hour)
                             ))
                                                                                    //=======
            );
            var termsAggregation = searchResponse23.Aggregations.DateHistogram("dialy")?.Buckets?.ToList();
            var result = new List<DocumentReport>();
            foreach (var item in termsAggregation)
            {
                var r = result.FirstOrDefault(t => t.Key == item.KeyAsString);
                result.Add(new DocumentReport(item.KeyAsString, item.DocCount.Value));
            }
            return result;
        }
        public IList<DailyReport> GetDailyInformation(DateTime? start = null, DateTime? end = null)
        {
            if (start == null)
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (end == null)
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            var dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                    v => v.Field(ConvertToCamelCase("requestTimestamp"))
                .GreaterThanOrEquals(start.Value.ToUniversalTime()));

            dateRangeForDateTime = dateRangeForDateTime && new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                v => v.Field(ConvertToCamelCase("requestTimestamp"))
            .LessThanOrEquals(end.Value.ToUniversalTime()));

            var searchResponse23 = client.Search<ApiLogEntryDto>(s => s
             .Query(t => dateRangeForDateTime)
             .Aggregations(a => a
             .DateHistogram("dialy", t => t.Field("requestTimestamp").Format("HH").TimeZone("Asia/Tehran").Interval(DateInterval.Hour)
             .Aggregations(da1 => da1.Sum("sumRequestContentLength", das => das.Field(dasf => dasf.RequestContentLength))
                               .Sum("sumResponseContentLength", das => das.Field(dasf => dasf.ResponseContentLength))
             ))
  //.Sum("sumRequest", sa => sa.Field(p => p.RequestContentLength))
  //.Sum("sumResponse", sa => sa.Field(p => p.ResponseContentLength))

  //=======
  ));
            var termsAggregation = searchResponse23.Aggregations.DateHistogram("dialy")?.Buckets?.ToList();

            var result = new List<DailyReport>();
            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    result.Add(new DailyReport("0" + i, 0, 0, 0));
                else
                    result.Add(new DailyReport("" + i, 0, 0, 0));
            }
            foreach (var item in termsAggregation)
            {
                var r = result.FirstOrDefault(t => t.Key == item.KeyAsString);
                r.DocumentCount += item.DocCount.Value;
                r.TotalRequestLength = item.SumBucket("sumRequestContentLength")?.Value;
                r.TotalResponseLength = item.SumBucket("sumResponseContentLength")?.Value;
            }
            var errorList = this.GetDailyError(start, end);
            foreach (var item in errorList)
            {
                result.FirstOrDefault(t => t.Key == item.Key).ErrorCount = item.DocumentCount;
            }
            return result;


            //            GET / labxand_logger3 / _search
            //{
            //                "size": 0, 
            //"aggs": {
            //                    "Group By Date": {
            //                        "date_histogram": {
            //                            "field": "requestTimestamp",
            //        "interval": "hour",
            //        "format" : "HH",
            //        "time_zone": "Asia/Tehran"
            //                           }
            //                    }
            //                }

            //            }
        }
        public IList<DocumentReport> GetResponseStatusInformation(DateTime? start = null, DateTime? end = null)
        {
            return this.GetInformationByFiledName("responseStatusCode", 20, start, end);
        }
        public IList<DocumentReport> GetBrowserName(DateTime? start, DateTime? end)
        {
            return GetInformationByFiledName("browserName", 10, start, end);
        }


        private IList<DocumentReport> GetInformationByFiledName(string fieldName, int size, DateTime? start = null, DateTime? end = null)
        {
            QueryContainer dateRangeForDateTime = null;
            if (start != null)
            {
                dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                  v => v.Field(ConvertToCamelCase("requestTimestamp"))
                        .GreaterThanOrEquals(start.Value.ToUniversalTime()));
            }
            if (end != null)
            {
                if (dateRangeForDateTime == null)
                    dateRangeForDateTime = new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                 v => v.Field(ConvertToCamelCase("requestTimestamp"))
                       .LessThanOrEquals(end.Value.ToUniversalTime()));
                else
                    dateRangeForDateTime = dateRangeForDateTime && new QueryContainerDescriptor<ApiLogEntryDto>().DateRange(
                   v => v.Field(ConvertToCamelCase("requestTimestamp"))
                         .LessThanOrEquals(end.Value.ToUniversalTime()));
            }



            try
            {
                var feilName2 = Char.ToUpperInvariant(fieldName[0]) + fieldName.Substring(1);
                var sortType = new ApiLogEntryDto().GetType().GetProperty(feilName2);
                if (sortType != null && sortType.PropertyType.Name == "String")
                    fieldName += ".keyword";
                var searchResponse23 = client.Search<ApiLogEntryDto>(s => s
             .Query(t => dateRangeForDateTime)
             .Aggregations(a => a
             .Terms("status", t => t.Field(fieldName).Size(size)
                //=======
             )));

                var termsAggregation = searchResponse23.Aggregations.Terms("status")?.Buckets?.ToList();

                var result = new List<DocumentReport>();

                foreach (var item in termsAggregation)
                {
                    result.Add(new DocumentReport(item.Key, item.DocCount.Value));
                }

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IList<DocumentReport> GetSubSystemFromElasticSearch()
        {
            var test = this.GetInformationByFiledName("application", 50);
            return test;
        }
        public IList<DocumentReport> GetControllerNameFromElasticSearch()
        {
            var test = this.GetInformationByFiledName("controllerName", 500);
            return test;
        }
    }
}
