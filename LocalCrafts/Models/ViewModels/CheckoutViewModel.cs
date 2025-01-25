namespace LocalCrafts.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItem> CartItems { get; set; }  
        public string FirstName { get; set; }         
        public string LastName { get; set; }           
        public string Email { get; set; }              
        public string PhoneNumber { get; set; }        
        public string City { get; set; }               
        public string Address { get; set; }            
        public string? AdditionalInfo { get; set; }     
        public string PaymentMethod { get; set; }     
        public decimal TotalAmount { get; set; }
    }

}
