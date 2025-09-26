using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;

namespace LabXand.Logging.Core
{
    [DataContract]
    [Serializable]
    public class ServiceLogEntry : ILogEntry
    {
        protected ServiceLogEntry()
        {
        }
        public ServiceLogEntry(ServiceOperationTypes operationType)
        {
            Type = operationType;
        }
        [NonSerialized]
        private System.Diagnostics.Stopwatch watch = null;
        public void Initiate(IRootLogEntry parentEntity, Guid unitOfWorkInstanceId)
        {
            watch = Stopwatch.StartNew();
            //this.Id = Guid.NewGuid();
            StartTime = DateTime.Now;
            ParentEntry = (ApiLogEntry)parentEntity;
            UnitOfWorkInstanceId = unitOfWorkInstanceId;
        }

        public void SetMethodParameters(int methodCallIndex)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(methodCallIndex);
            if (stackFrame != null)
            {
                MethodBase currentMethodName = stackFrame.GetMethod();
                if (currentMethodName != null)
                {
                    if (currentMethodName.ReflectedType != null || currentMethodName.DeclaringType != null)
                    {
                        this.ServiceName = currentMethodName.ReflectedType != null?
                            currentMethodName.ReflectedType.Name : currentMethodName.DeclaringType.Name;
                        var serviceDesciptorAttribute =
                            currentMethodName.GetCustomAttributes(typeof(ServiceDesciptorAttribute), true);

                        if (serviceDesciptorAttribute != null && serviceDesciptorAttribute.Length > 0)
                        {
                            ServiceDesciptorAttribute descriptor = (ServiceDesciptorAttribute)serviceDesciptorAttribute[0];

                            if (descriptor != null)
                            {
                                this.Description = descriptor.Description;
                                this.Type = descriptor.OperationType; 
                            }
                        }
                        else
                            this.Description = ServiceName;
                    }

                }

            }

        }
       // [DataMember]

        public ApiLogEntry ParentEntry { get; set; }
        public void Success()
        {
            Complete(ServiceOperationStatus.Success);
        }
        public void Error(Exception exception)
        {
            Complete(ServiceOperationStatus.Error, exception);
        }
        public void SetUserInformation(string supplementaryUserInformation)
        {
            this.ParentEntry.SupplementaryUserInformation = supplementaryUserInformation;
        }
        protected void Complete(ServiceOperationStatus status, Exception exception = null)
        {
            watch.Stop();
            this.ElapsedTime = watch.ElapsedMilliseconds;
            this.CompleteTime = DateTime.Now;
            this.Status = status;
            //if (status == ServiceOperationStatus.Error)
            //{
            //    OperationExceptionMessage = ExceptionHandlerFactory.GetSuitableHandler(exception).GetTechnicalDetails(exception);
            //    SerializeException(exception);
            //}
        }
        [DataMember]

        public long Id { get; private set; }
        [DataMember]

        public Guid UnitOfWorkInstanceId { get; private set; }
        /// <summary>
        /// Success
        /// Error
        /// </summary>
        [DataMember]
        public ServiceOperationStatus Status { get; set; }
        /// <summary>
        /// Fetch
        /// Save
        /// Delete
        /// </summary>
        [DataMember]
        public ServiceOperationTypes Type { get; set; }
        //public string UserId { get; private set; }
        //public string UserName { get; private set; }        
        [DataMember]
        public string ServiceName { get; private set; }
        [DataMember]
        public string Description { get; private set; }
        [DataMember]
        public string OperationName { get; private set; }
        [DataMember]
        public DateTime StartTime { get; private set; }
        [DataMember]
        public DateTime CompleteTime { get; private set; }
        [DataMember]
        public long ElapsedTime { get; private set; }
        //public List<KeyValuePair<string, string>> InputParameters { get; set; }
        //public KeyValuePair<string, string> OutPutParameters { get; set; }
        [DataMember]
        public List<DataBaseLogEntry> Details { get; set; }
        //public byte[] OperationException { get; private set; }
        //public string OperationExceptionMessage { get; private set; }
    }
}
