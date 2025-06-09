namespace WebAppEF.AdventureS.Models.Filters;

public class WorkOrderRoutingFilter
{
    public int[] IDs { get; set; } = [];

    public double PlannedCost { get; set; } = 0.0;

    public double ActualCost { get; set; } = 0.0;
}
