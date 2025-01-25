using LocalCrafts.Models;
using System.Collections.Generic;

namespace LocalCrafts.Models.ViewModels
{
    public class SellerProductsViewModel
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerBio { get; set; }
        public string SellerImage { get; set; }
        public List<Product> Products { get; set; }
        public int UserId { get; set; }
    }
}
