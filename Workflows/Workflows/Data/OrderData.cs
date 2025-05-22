namespace WorkflowsEx.Data;

public class OrderData
{
    public string OrderId { get; set; }

    public bool InventoryReserved { get; set; }
    
    public bool PaymentCharged { get; set; }
}
