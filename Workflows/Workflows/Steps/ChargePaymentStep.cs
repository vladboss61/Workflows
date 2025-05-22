namespace WorkflowsEx.Steps;

using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

public sealed class ChargePaymentStep : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        try
        {
            Console.WriteLine("Payment charged");
        }
        finally
        {
            // simulate error
            throw new Exception("Payment failed");
        }
    }
}
