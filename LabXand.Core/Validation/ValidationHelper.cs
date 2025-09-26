using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LabXand.Core.Validation
{
    public static class ValidationHelper
    {
        public static void Validate(object validatableObject)
        {
            var context = new ValidationContext(validatableObject, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(validatableObject, context, results, true);

            StringBuilder sb = new StringBuilder();
            foreach (var item in results)
            {
                sb.AppendLine(item.ErrorMessage);
            }
            if (sb.Length > 0)
                throw new ValidationException(sb.ToString());
        }
    }
}
