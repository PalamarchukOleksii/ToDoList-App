using System.ComponentModel.DataAnnotations;

namespace API.CustomAttribute
{
    public class FutureOrNowDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime dateTime;
            if (value is DateTime)
            {
                dateTime = (DateTime)value;
                return dateTime >= DateTime.Now;
            }

            return false;
        }
    }
}