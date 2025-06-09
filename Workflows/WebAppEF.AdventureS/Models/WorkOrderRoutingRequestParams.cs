using System.ComponentModel.DataAnnotations;
using WebAppEF.AdventureS.Controllers;
using WebAppEF.AdventureS.Models.Filters;

namespace WebAppEF.AdventureS.Models;

public class WorkOrderRoutingRequestParams
{
    public WorkOrderRoutingFilter Filter { get; set; }

    public string SortField { get; set; } = "OperationSequence";

    [RegularExpression($"{nameof(Order.Asc)}|{nameof(Order.Desc)}")]
    public Order SortOrder { get; set; } = Order.Desc; // Default sort order, can be "ASC" or "DESC"

    [Range(1, int.MaxValue)]
    public int PageIndex { get; set; } = 1;

    [Range(0, int.MaxValue)]
    public int PageSize { get; set; } = 10; // Default page size
}
