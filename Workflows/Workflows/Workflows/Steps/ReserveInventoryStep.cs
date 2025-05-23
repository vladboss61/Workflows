namespace WorkflowsEx.Workflows.Steps;

using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public sealed class ReserveInventoryStep : StepBody
{
    public string OrderId { get; set; }

    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine($"Inventory reserved | OrderId {OrderId}");
        return ExecutionResult.Next();
    }
}
