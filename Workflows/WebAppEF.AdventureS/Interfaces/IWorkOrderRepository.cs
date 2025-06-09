using System.Threading.Tasks;
using WebAppEF.AdventureS.Models;

namespace WebAppEF.AdventureS.Interfaces;

public interface IWorkOrderRepository
{
    public Task<PagedResult<WorkOrderRouting>> GetWorkOrderRoutingsAsync(WorkOrderRoutingRequestParams workOrderRoutingRequestParams);
}
