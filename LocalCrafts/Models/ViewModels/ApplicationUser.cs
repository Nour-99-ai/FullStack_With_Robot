using Microsoft.AspNetCore.Identity;
namespace LocalCrafts.Models.ViewModels
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
      

    }
}
