using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Attributes.Validators
{

  public class ValidEnum : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if(!(value is Enum))
      {
        return new ValidationResult("This type is not compatible with Enum");
      }

      if(!Enum.IsDefined(value.GetType(), value))
      {
        return new ValidationResult("Enum is out of range");
      }

      return ValidationResult.Success;
    }
  }

}
