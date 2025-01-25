namespace LocalCrafts.Models.ViewModels
{
    public class SearchResultsViewModel
    {
        public string Query { get; set; }
        public List<Category> Categories { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Product> Products { get; set; }
    }

}
