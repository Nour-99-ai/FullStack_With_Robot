using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalCrafts.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Enter Category Name")]
        public string? CategoryName { get; set; }

        public string? Image { get; set; }



        public List<Product>? Products { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; } // For file upload
    }
}
