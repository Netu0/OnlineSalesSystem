using System.ComponentModel.DataAnnotations;

namespace OnlineSalesSystem.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        public required Customer Customer { get; set; }
        
        [Required]
        private DateTime _orderDate;
        
        public DateTime OrderDate
        {
            get => _orderDate;
            set => _orderDate = value.Kind == DateTimeKind.Unspecified 
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc) 
                : value.ToUniversalTime();
        }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Total { get; set; }
    }
}