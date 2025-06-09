using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebAppEF.AdventureS.Controllers;

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

public class DbWorkOrderRoutingParams
{
    public double? PlannedCost { get; set; }

    public double? ActualCost { get; set; }

    public int[] IDs { get; set; }

    public int Offset { get; set; }

    public int PageSize { get; set; } = 10; // Default page size
}

public enum Order
{
    Asc,
    Desc
}

public class WorkOrderRoutingFilter
{
    public int[] IDs { get; set; } = [];

    public double PlannedCost { get; set; } = 0.0;

    public double ActualCost { get; set; } = 0.0;
}

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

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }

    public int TotalCount { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}


[ApiController]
[Route("work-order")]
public class WorkOrdersController : ControllerBase
{
    private readonly ILogger<WorkOrdersController> _logger;
    private readonly IDbConnection _dbConnection;

    public WorkOrdersController(IDbConnection dbConnection, ILogger<WorkOrdersController> logger)
    {
        _dbConnection = dbConnection;
        _logger = logger;
    }

    [HttpPost("work-order-routings")]
    public async Task<PagedResult<WorkOrderRouting>> GetWorkOrderRoutingsAsync([FromBody] WorkOrderRoutingRequestParams workOrderRoutingRequestParams)
    {
        var propertiesInfo = typeof(WorkOrderRouting).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        string[] availableSortedProperties = propertiesInfo.Select(x => x.Name).ToArray();

        bool allowSort = availableSortedProperties.Any(x => string.Equals(workOrderRoutingRequestParams.SortField, x));

        if (!allowSort)
        {
            throw new InvalidOperationException("Bad sort parameter");
        }

        var stringBuilder = new StringBuilder("WHERE 1=1");

        var workOrderRoutingParams = new DbWorkOrderRoutingParams
        {
            Offset = (workOrderRoutingRequestParams.PageIndex - 1) * workOrderRoutingRequestParams.PageSize,
            PageSize = workOrderRoutingRequestParams.PageSize
        };

        if (workOrderRoutingRequestParams.Filter != null)
        {
            if (workOrderRoutingRequestParams.Filter.IDs?.Length > 0)
            {
                workOrderRoutingParams.IDs = workOrderRoutingRequestParams.Filter.IDs;
                stringBuilder.Append(" AND OperationSequence IN @IDs");
            }

            if (workOrderRoutingRequestParams.Filter.PlannedCost > 0.0)
            {
                workOrderRoutingParams.PlannedCost = workOrderRoutingRequestParams.Filter.PlannedCost;
                stringBuilder.Append(" AND PlannedCost >= @PlannedCost");
            }

            if (workOrderRoutingRequestParams.Filter.ActualCost > 0.0)
            {
                workOrderRoutingParams.ActualCost = workOrderRoutingRequestParams.Filter.ActualCost;
                stringBuilder.Append(" AND ActualCost >= @ActualCost");
            }
        }

        var sql = $"""
                SELECT
                    WorkOrderID,
                    ProductID,
                    LocationID,
                    ScheduledStartDate,
                    OperationSequence,
                    ActualCost,
                    PlannedCost,
                    COUNT(*) OVER() AS TotalCount
                FROM Production.WorkOrderRouting
                {stringBuilder}
                ORDER BY {workOrderRoutingRequestParams.SortField} {workOrderRoutingRequestParams.SortOrder.ToString().ToUpper()}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*) FROM Production.WorkOrderRouting {stringBuilder};
            """;

        SqlMapper.GridReader reader = await _dbConnection.QueryMultipleAsync(sql, workOrderRoutingParams);

        IEnumerable<WorkOrderRouting> workOrderRoutings = reader.Read<WorkOrderRouting>();
        int totalCount = reader.ReadFirst<int>();

        return new PagedResult<WorkOrderRouting>
        {
            Items = workOrderRoutings ?? [],
            TotalCount = totalCount,
            PageIndex = workOrderRoutingRequestParams.PageIndex,
            PageSize = workOrderRoutingRequestParams.PageSize
        };
    }
}
