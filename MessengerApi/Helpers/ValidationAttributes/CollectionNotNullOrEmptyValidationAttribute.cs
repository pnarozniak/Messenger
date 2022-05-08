using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Helpers.ValidationAttributes
{
    public class CollectionNotNullOrEmptyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   
            if (value is null)
            {
                return new ValidationResult("Collection is null");
            }

            if (value is ICollection collection)
            {
                if (collection.Count == 0)
                {
                    return new ValidationResult("Collection is empty");
                }
            }

            return ValidationResult.Success;
        }
    }
}