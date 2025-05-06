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

public class OrderResponseDTO
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
}