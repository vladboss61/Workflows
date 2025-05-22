using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowsEx.Steps;

public sealed class ChargePayment : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        try
        {
            Console.WriteLine("Payment charged");
        } finally
        {
            // simulate error
            throw new Exception("Payment failed");
        }
    }
}
