using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LabXand.Core.Validation
{
    public class CustomeRequiredAttribute : RequiredAttribute
    {
        private String displayName;

        public CustomeRequiredAttribute()
        {
            this.ErrorMessage = "{0} is required";
            this.AllowEmptyStrings = false;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
