using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalCrafts.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Seller Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bio is required.")]
        [StringLength(500, ErrorMessage = "Bio cannot be longer than 500 characters.")]
        public string Bio { get; set; }

        public string? Image { get; set; }

        public List<Product>? Products { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; } // For file upload

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }  

        [Required]
        public int UserId { get; set; }
        public bool IsCompleted { get; set; }

    }
}
