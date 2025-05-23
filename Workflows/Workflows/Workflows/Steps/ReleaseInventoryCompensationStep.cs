namespace WorkflowsEx.Workflows.Steps;

using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public sealed class ReleaseInventoryCompensationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Inventory released (compensation)");
        return ExecutionResult.Next();
    }
}
