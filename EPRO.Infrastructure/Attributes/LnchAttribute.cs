using EPRO.Infrastructure.HelperClasses;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Attributes
{
    public class LnchAttribute : ValidationAttribute
    {
        public LnchAttribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            BasicLncValidation lnchValidation = new BasicLncValidation(value.ToString());

            if (!lnchValidation.Validate())
            {
                return new ValidationResult(lnchValidation.ErrorMessage);
            }

            return null;
        }
    }
}
