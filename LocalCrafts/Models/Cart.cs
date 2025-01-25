using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalCrafts.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string? UserId { get; set; }  

        public List<CartItem> Items { get; set; }

        public decimal Total { get; set; }
    }

    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public int CartId { get; set; }  
        public Cart Cart { get; set; }
        public int SellerId { get; set; }

        public Product Product { get; set; }
    }


}
