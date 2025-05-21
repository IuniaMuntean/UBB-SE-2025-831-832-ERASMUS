using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Services.interfaces
{
    internal interface IRoadFinancialsService
    {
        Task GetDistanceAsync(int id);
        Task GetCostAsync(int id);
        Task GetRevenueAsync(int id);
        Task GetFeeEurosAsync(int id);
        Task GetEstimatedTimeArrivalAsync(int id);
        Task CalculateProfitAsync(int id);
        Task CalculatePrice(int id);
    }
}
