using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eShop.AdminApp.Components.Validation;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string conditionProperty;
    private readonly object conditionValue;

    public RequiredIfAttribute(string conditionProperty, object conditionValue)
    {
        this.conditionProperty = conditionProperty;
        this.conditionValue = conditionValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        PropertyInfo? property = validationContext.ObjectType.GetProperty(conditionProperty);
        if (property == null)
        {
            return new ValidationResult($"Unknown property: {conditionProperty}");
        }

        object? conditionValue = property.GetValue(validationContext.ObjectInstance);
        if (this.conditionValue.Equals(conditionValue) && value == null)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
        }

        return ValidationResult.Success;
    }
}
