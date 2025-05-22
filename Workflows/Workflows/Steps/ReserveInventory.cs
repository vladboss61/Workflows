using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowsEx.Steps;

public sealed class ReserveInventory : StepBody
{
    public string OrderId { get; set; }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine($"Inventory reserved | OrderId {OrderId}");
        return ExecutionResult.Next();
    }
}
