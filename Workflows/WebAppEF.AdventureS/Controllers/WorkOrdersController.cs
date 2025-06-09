using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;

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

    [RegularExpression("WorkOrderID|ProductID|LocationID|ScheduledStartDate|OperationSequence")]
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
    public ICollection<T> Items { get; set; }

    public int TotalCount { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}


[ApiController]
[Route("work-order")]
public class WorkOrdersController : ControllerBase
{
    private readonly ConnectionStrings _options;
    private readonly ILogger<WorkOrdersController> _logger;
    public readonly IDbConnection _dbConnection;

    public WorkOrdersController(
        IDbConnection dbConnection,
        IOptions<ConnectionStrings> options,
        ILogger<WorkOrdersController> logger)
    {
        _dbConnection = dbConnection;
        _options = options.Value;
        _logger = logger;
    }

    [HttpPost("work-order-routings")]
    public PagedResult<WorkOrderRouting> GetWorkOrderRoutings([FromBody] WorkOrderRoutingRequestParams workOrderRoutingRequestParams)
    {
        var sql = $"""
                SELECT
                    WorkOrderID,
                    ProductID,
                    LocationID,
                    ScheduledStartDate,
                    OperationSequence
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

        SqlMapper.GridReader reader = _dbConnection.QueryMultiple(sql, workOrderRoutingParams);

        var workOrderRoutings = reader.Read<WorkOrderRouting>().ToList();
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
