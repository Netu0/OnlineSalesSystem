using System.ComponentModel.DataAnnotations;

namespace OnlineSalesSystem.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Name { get; set; } // Adicionado 'required'
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; } // Adicionado 'required'
        
        [Phone]
        public string? Phone { get; set; } // Tornado nullable com '?'
        
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}