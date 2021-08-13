using EPRO.Infrastructure.HelperClasses;
using System.ComponentModel.DataAnnotations;

namespace EPRO.Infrastructure.Attributes
{
    public class EgnAttribute : ValidationAttribute
    {
        public EgnAttribute()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            BasicEGNValidation egnValidation = new BasicEGNValidation(value.ToString());

            if (!egnValidation.Validate())
            {
                return new ValidationResult(egnValidation.ErrorMessage);
            }
            
            return null;
        }
    }
}
