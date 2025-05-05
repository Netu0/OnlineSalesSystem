using System.ComponentModel.DataAnnotations;

namespace OnlineSalesSystem.Api.DTOs;

public class OrderCreateDTO
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }
}

public class OrderUpdateDTO
{
    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }
}