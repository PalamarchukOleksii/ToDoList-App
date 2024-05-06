using System.ComponentModel.DataAnnotations;

namespace API.CustomAttribute
{
    public class ValidBool : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (!bool.TryParse(value.ToString(), out _))
            {
                return false;
            }

            return true;
        }
    }
}