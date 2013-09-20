using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ILB
{
    public class ValidationResults
    {
        public ValidationResults(IEnumerable<ValidationResult> results)
        {
            var validationResults = results as ValidationResult[] ?? results.ToArray();
            IsValid = !validationResults.Any();
            Results = validationResults;
        }

        public bool IsValid { get; private set; }
        public IEnumerable<ValidationResult> Results { get; private set; }
    }
}