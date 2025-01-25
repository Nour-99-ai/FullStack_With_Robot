using System;
using System.Collections.Generic;

namespace LocalCrafts.Models
{
    public class Order
    {
        public int OrderId { get; set; }         
        public string UserId { get; set; }       
        public string FirstName { get; set; }    
        public string LastName { get; set; }     
        public string Email { get; set; }        
        public string PhoneNumber { get; set; }  
        public string City { get; set; }         
        public string Address { get; set; }      
        public string? AdditionalInfo { get; set; } 

        public string PaymentMethod { get; set; } 
        public decimal TotalAmount { get; set; }  
        public DateTime OrderDate { get; set; }   

        public List<OrderItem> OrderItems { get; set; }  
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }   
        public int? OrderId { get; set; }
        public int ProductId { get; set; }      
        public string ProductName { get; set; } 
        public decimal Price { get; set; }      
        public int Quantity { get; set; }       
        public decimal Subtotal { get; set; }   
        public int SellerId { get; set; }       
        public string OrderItemStatus { get; set; } = "Pending"; 
        public DateTime? DeliveryDate { get; set; }
        public Product Product { get; set; }    
        public Order Order { get; set; }        
    }
}
