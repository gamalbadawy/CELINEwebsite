using System.ComponentModel.DataAnnotations;

namespace CELINEwebsite.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Message field is required")]
        public string Message { get; set; }
    }
}