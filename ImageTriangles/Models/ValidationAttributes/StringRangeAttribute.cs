using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models.ValidationAttributes
{
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            string msg = ErrorMessage;
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                msg = $"Please enter one of the allowable values: {string.Join(", ", AllowableValues ?? new[] { "No allowable values found" })}.";
            }

            return new ValidationResult(msg);
        }
    }
}