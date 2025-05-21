using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    internal class RoadFinancialsService : IRoadFinancialsService
    {
        private readonly TransportDbContext _context;

        public RoadFinancialsService(TransportDbContext context)
        {
            _context = context;
        }

        public async Task CalculatePrice(int id)
        {
            throw new NotImplementedException();
            //var route = _context.Pricings
              //  .FirstOrDefaultAsync(p => p.id == id);
             //.GetRevenue() - route.GetCost();
        }

        public async Task CalculateProfitAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetCostAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetDistanceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetEstimatedTimeArrivalAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetFeeEurosAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetRevenueAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
