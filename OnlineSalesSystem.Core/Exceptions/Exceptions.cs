namespace OnlineSalesSystem.Core.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(int customerId) 
        : base($"Customer with ID {customerId} was not found") { }
}

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(int orderId) 
        : base($"Order with ID {orderId} was not found") { }
}

public class InvalidOrderOperationException : Exception
{
    public InvalidOrderOperationException(string message) 
        : base(message) { }
}