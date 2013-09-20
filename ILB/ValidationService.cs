using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ILB
{
    public class ValidationService : IValidationService
    {
        public ValidationResults Validate(object obj)
        {
            var context = new ValidationContext(obj);
            var validationResults = new Collection<ValidationResult>();
            Validator.TryValidateObject(obj, context, validationResults);
            return new ValidationResults(validationResults);
        }
    }
}