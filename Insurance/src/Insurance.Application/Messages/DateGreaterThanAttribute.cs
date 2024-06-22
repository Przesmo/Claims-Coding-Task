using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages;

internal class DateGreaterThanAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public DateGreaterThanAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult(ErrorMessage ?? "Make sure your date is not null");
        }

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property is null)
        {
            throw new Exception("Ivanlid property");
        }

        var propertyValue = property.GetValue(validationContext.ObjectInstance);
        if (propertyValue is null)
        {
            return new ValidationResult(ErrorMessage = "End date must be later than start date");
        }

        var comparisonValue = (DateTime)propertyValue;
        var currentValue = (DateTime)value;
        if (currentValue < comparisonValue)
        {
            return new ValidationResult(ErrorMessage = "End date must be later than start date");
        }

        return ValidationResult.Success!;
    }
}
