using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages;

public class DateIsGreaterThanTodayAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value,
        ValidationContext validationContext)
    {
        if(value is null)
        {
            return new ValidationResult(ErrorMessage ?? "Make sure your date is not null");
        }

        DateTime dt = (DateTime)value;
        if (dt.Date >= DateTime.UtcNow.Date)
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult(ErrorMessage ?? "Make sure your date is >= than today");
    }

}
