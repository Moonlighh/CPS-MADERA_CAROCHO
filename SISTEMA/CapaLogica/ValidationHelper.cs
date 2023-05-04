using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public static class ValidationHelper
{
    public static bool TryValidateEntityMsj<T>(T entity, out List<string> errors)
    {
        var validationContext = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);
        errors = validationResults.Select(r => r.ErrorMessage).ToList();

        return isValid;
    }
    public static bool TryValidateEntity<T>(T entity)
    {
        var validationContext = new ValidationContext(entity);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

        return isValid;
    }
}
