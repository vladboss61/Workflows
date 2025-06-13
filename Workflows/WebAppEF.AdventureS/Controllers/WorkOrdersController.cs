using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAppEF.AdventureS.Ef;
using WebAppEF.AdventureS.Interfaces;
using WebAppEF.AdventureS.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebAppEF.AdventureS.Controllers;

[ApiController]
[Route("work-order")]
public class WorkOrdersController : ControllerBase
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<WorkOrdersController> _logger;
    private readonly IWorkOrderRepository _workOrderRepository;

    public WorkOrdersController(
        ApplicationDbContext applicationDbContext,
        IWorkOrderRepository workOrderRepository,
        ILogger<WorkOrdersController> logger)
    {
        _applicationDbContext = applicationDbContext;
        _workOrderRepository = workOrderRepository;
        _logger = logger;
    }

    [HttpPost("work-order-routings")]
    public Task<PagedResult<WorkOrderRouting>> GetWorkOrderRoutingsAsync([FromBody] WorkOrderRoutingRequestParams workOrderRoutingRequestParams)
    {
        return _workOrderRepository.GetWorkOrderRoutingsAsync(workOrderRoutingRequestParams);
    }
}
