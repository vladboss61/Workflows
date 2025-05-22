namespace WorkflowsEx.Workflows;

using WorkflowsEx.Data;
using WorkflowCore.Interface;
using WorkflowsEx.Steps;

public sealed class OrderWorkflow : IWorkflow<OrderData>
{
    public string Id => nameof(OrderWorkflow);

    public int Version => 1;

    public void Build(IWorkflowBuilder<OrderData> builder)
    {
        builder
            .StartWith<ReserveInventory>()
                .Input((step, data) => step.OrderId = data.OrderId)
                .CompensateWith<ReleaseInventory>()
            .Then<ChargePayment>()
                .CompensateWith<RefundPayment>();
    }
}
