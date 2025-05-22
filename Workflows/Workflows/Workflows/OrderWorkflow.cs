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
            .StartWith<ReserveInventoryStep>()
                .Input((step, data) => step.OrderId = data.OrderId)
                .CompensateWith<ReleaseInventoryCompensationStep>()
            .Then<ChargePaymentStep>()
                .CompensateWith<RefundPaymentCompensationStep>();
    }
}
