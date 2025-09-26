using LabXand.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Infrastructure.Data.Exceptions
{

    [Serializable]
    public class IlligalModificationException : ExceptionBase
    {
        const string MESSAGE = "این رکورد توسط '{0}' ایجاد شده و نمی توان با '{1}' ویرایش شود.";
        public IlligalModificationException(string creator, string editor) : base(string.Format(MESSAGE, creator, editor)) { }
    }
}
