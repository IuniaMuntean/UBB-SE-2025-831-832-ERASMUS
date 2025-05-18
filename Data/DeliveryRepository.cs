using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Data
{
    public class DeliveryRepository : Repository<Delivery>
    {
        private readonly TransportDbContext _context;

        public DeliveryRepository(TransportDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                    .ThenInclude(o => o.SourceCity)
                .Include(d => d.Order)
                    .ThenInclude(o => o.DestinationCity)
                .Include(d => d.Driver)
                .Include(d => d.Truck)
                .ToListAsync();
        }
    }
} 