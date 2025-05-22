using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowsEx.Steps;

public sealed class ReleaseInventory : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Inventory released (compensation)");
        return ExecutionResult.Next();
    }
}
