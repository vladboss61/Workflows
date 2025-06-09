namespace WebAppEF.AdventureS.Models.Filters;

public class DbWorkOrderRoutingParams
{
    public double? PlannedCost { get; set; }

    public double? ActualCost { get; set; }

    public int[] IDs { get; set; }

    public int Offset { get; set; }

    public int PageSize { get; set; } = 10; // Default page size
}
