using System.ComponentModel.DataAnnotations;

namespace LocalCrafts.Models
{
    public class Contactus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
