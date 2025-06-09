using Dapper;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAppEF.AdventureS.Interfaces;
using WebAppEF.AdventureS.Models;
using WebAppEF.AdventureS.Models.Filters;

namespace WebAppEF.AdventureS.Services;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly IDbConnection _dbConnection;

    public WorkOrderRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<PagedResult<WorkOrderRouting>> GetWorkOrderRoutingsAsync(WorkOrderRoutingRequestParams workOrderRoutingRequestParams)
    {
        var propertiesInfo = typeof(WorkOrderRouting).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        string[] availableSortedProperties = propertiesInfo.Select(x => x.Name).ToArray();

        bool allowSort = availableSortedProperties.Any(x => string.Equals(workOrderRoutingRequestParams.SortField, x));

        if (!allowSort)
        {
            throw new InvalidOperationException("Bad sort parameter");
        }

        var filterBuilder = new StringBuilder("WHERE 1=1");

        var workOrderRoutingParams = new DbWorkOrderRoutingParams
        {
            Offset = (workOrderRoutingRequestParams.PageIndex - 1) * workOrderRoutingRequestParams.PageSize,
            PageSize = workOrderRoutingRequestParams.PageSize
        };

        ApplyFilters(workOrderRoutingRequestParams.Filter, workOrderRoutingParams, filterBuilder);

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
                {filterBuilder}
                ORDER BY {workOrderRoutingRequestParams.SortField} {workOrderRoutingRequestParams.SortOrder.ToString().ToUpper()}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                SELECT COUNT(*) FROM Production.WorkOrderRouting {filterBuilder};
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

    public void ApplyFilters(WorkOrderRoutingFilter filter, DbWorkOrderRoutingParams workOrderRoutingParams, StringBuilder filterBuilder)
    {
        if (filter is null)
        {
            return;
        }

        if (filter.IDs?.Length > 0)
        {
            workOrderRoutingParams.IDs = filter.IDs;
            filterBuilder.Append(" AND OperationSequence IN @IDs");
        }

        if (filter.PlannedCost > 0.0)
        {
            workOrderRoutingParams.PlannedCost = filter.PlannedCost;
            filterBuilder.Append(" AND PlannedCost >= @PlannedCost");
        }

        if (filter.ActualCost > 0.0)
        {
            workOrderRoutingParams.ActualCost = filter.ActualCost;
            filterBuilder.Append(" AND ActualCost >= @ActualCost");
        }
    }
}
