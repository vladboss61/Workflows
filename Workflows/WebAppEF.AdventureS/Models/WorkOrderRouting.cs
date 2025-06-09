using System;

namespace WebAppEF.AdventureS.Models;

public class WorkOrderRouting
{
    public int WorkOrderID { get; set; }
    public int ProductID { get; set; }
    public int OperationSequence { get; set; }
    public int LocationID { get; set; }
    public DateTime ScheduledStartDate { get; set; }
    public DateTime ScheduledEndDate { get; set; }
    public DateTime ActualStartDate { get; set; }
    public DateTime ActualEndDate { get; set; }
    public decimal ActualResourceHrs { get; set; }
    public decimal ActualCost { get; set; }
    public decimal PlannedCost { get; set; }
    public int TotalCount { get; set; }
}