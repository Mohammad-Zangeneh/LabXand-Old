using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Logging.LogService
{
    public class IMasUserContextForLog
    {

        // public List<string> AuthorizedOperations { get; set; }
        //public Security.ConfidentialityLevels ConfidentialityLevel { get; set; }
        public Guid? ContactId { get; set; }
        public string FirstName { get; set; }
        public long GeoUnitId { get; set; }
        //public IIdentity Identity { get; }
        public bool IsAgencyUser { get; protected set; }
        public bool IsConstructorUser { get; protected set; }
        public bool? IsNorthPort { get; set; }
        public bool IsOwnerUser { get; protected set; }
        public string LastName { get; set; }
        public string OrganizationCode { get; set; }
        public Guid OrganizationId { get; set; }
        public short ProvinceId { get; set; }
        public string SupportSystemToken { get; set; }
        public string Token { get; set; }
        public string UnitCode { get; set; }
        public Guid UnitId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        //public Security.UserTypes UserType { get; set; }

        //public override bool IsAuthorizedFor(string claim);
        //public bool IsInRole(string role);
        //public bool IsNull();
        //public override string ToString();
    }
}
