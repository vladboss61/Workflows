using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowsEx.Steps;

public sealed class RefundPayment : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Payment refunded (compensation)");
        return ExecutionResult.Next();
    }
}
