using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAppEF.AdventureS.Interfaces;
using WebAppEF.AdventureS.Models;

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
