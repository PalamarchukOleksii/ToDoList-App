using System.ComponentModel.DataAnnotations;
using API.CustomAttribute;

namespace API.Dtos.UserTask
{
    public class UserTaskUpdateDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Short info must be 3 characters")]
        [MaxLength(50, ErrorMessage = "Short info cannot be over 50 characters")]
        public string ShortInfo { get; set; } = string.Empty;
        [Required]
        [MinLength(3, ErrorMessage = "Full info must be 3 characters")]
        [MaxLength(200, ErrorMessage = "Full info cannot be over 200 characters")]
        public string FullInfo { get; set; } = string.Empty;
        [Required]
        [ValidBool(ErrorMessage = "Invalid value. Please select true or false.")]
        public bool IsDone { get; set; }
        [Required]
        [FutureOrNowDate(ErrorMessage = "End date and time must be in the future or today")]
        public DateTime EndDateTime { get; set; }
    }
}