namespace WorkflowsEx.Steps;

using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public sealed class RefundPaymentCompensationStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        Console.WriteLine("Payment refunded (compensation)");
        return ExecutionResult.Next();
    }
}
