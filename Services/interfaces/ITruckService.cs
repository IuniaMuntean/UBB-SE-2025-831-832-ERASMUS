using System.Collections.Generic;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services.interfaces
{
    public interface ITruckService
    {
        Task<IEnumerable<Truck>> GetAllAsync();
    }
} 