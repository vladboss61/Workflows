using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
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

    public string TotalCount { get; set; }
}

public class DbWorkOrderRoutingParams
{
    public int[] IDs { get; set; }

    public int Offset { get; set; }

    public int PageSize { get; set; } = 10; // Default page size
}

public class WorkOrderRoutingRequestParams
{
    public required int[] IDs { get; set; }

    public string SortField { get; set; } = "OperationSequence";

    [RegularExpression("ASC|DESC")]
    public string SortOrder { get; set; } = "ASC"; // Default sort order, can be "ASC" or "DESC"

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
        var s = typeof(WorkOrderRouting).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        string[] availableFilteredProperties = s.Select(x => x.Name).ToArray();

        bool allowSort = availableFilteredProperties.Any(x => string.Equals(workOrderRoutingRequestParams.SortField, x));

        if (!allowSort)
        {
            throw new InvalidOperationException("Bad sort parameter");
        }

        var sql = $"""
                SELECT
                    WorkOrderID,
                    ProductID,
                    LocationID,
                    ScheduledStartDate,
                    OperationSequence,
                    COUNT(*) OVER() AS TotalCount
                FROM Production.WorkOrderRouting
                WHERE OperationSequence IN @IDs
                ORDER BY {workOrderRoutingRequestParams.SortField} {workOrderRoutingRequestParams.SortOrder}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*) FROM Production.WorkOrderRouting WHERE OperationSequence IN @IDs;
            """;

        var workOrderRoutingParams = new DbWorkOrderRoutingParams
        {
            IDs = workOrderRoutingRequestParams.IDs,
            Offset = (workOrderRoutingRequestParams.PageIndex - 1) * workOrderRoutingRequestParams.PageSize,
            PageSize = workOrderRoutingRequestParams.PageSize
        };

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
