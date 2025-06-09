using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAppEF.AdventureS.Interfaces;
using WebAppEF.AdventureS.Models;
using WebAppEF.AdventureS.Models.Filters;

namespace WebAppEF.AdventureS.Controllers;

[ApiController]
[Route("work-order")]
public class WorkOrdersController : ControllerBase
{
    private readonly ILogger<WorkOrdersController> _logger;
    private readonly IWorkOrderRepository _workOrderRepository;

    public WorkOrdersController(IWorkOrderRepository workOrderRepository, ILogger<WorkOrdersController> logger)
    {
        _workOrderRepository = workOrderRepository;
        _logger = logger;
    }

    [HttpPost("work-order-routings")]
    public Task<PagedResult<WorkOrderRouting>> GetWorkOrderRoutingsAsync([FromBody] WorkOrderRoutingRequestParams workOrderRoutingRequestParams)
    {
        return _workOrderRepository.GetWorkOrderRoutingsAsync(workOrderRoutingRequestParams);
    }
}
