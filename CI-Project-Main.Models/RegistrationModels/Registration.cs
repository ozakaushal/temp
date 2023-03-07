using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CI_Project_Main.Models.RegistrationModels
{
    public class Registration
    {
        [Required(ErrorMessage = "This field is required")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "This field is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [NotNull]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [NotNull]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ConfirmPassword { get; set; }
    }
}
