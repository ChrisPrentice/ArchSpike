using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ILB
{
    public class ValidationService : IValidationService
    {
        public bool Validate(object obj)
        {
            var context = new ValidationContext(obj);
            return Validator.TryValidateObject(obj, context, new Collection<ValidationResult>());
        }
    }
}