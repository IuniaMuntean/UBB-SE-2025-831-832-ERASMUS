using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public class OrderService : IOrderService
    {
        private readonly TransportDbContext _context;

        public OrderService(TransportDbContext db)
        {
            _context = db;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.SourceCity)
                .Include(o => o.DestinationCity)
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            // Ensure cities exist
            if (order.SourceCity != null)
            {
                var sourceCity = await _context.Cities.FindAsync(order.SourceCity.id);
                if (sourceCity != null)
                {
                    order.SourceCity = sourceCity;
                }
            }

            if (order.DestinationCity != null)
            {
                var destCity = await _context.Cities.FindAsync(order.DestinationCity.id);
                if (destCity != null)
                {
                    order.DestinationCity = destCity;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.SourceCity)
                .Include(o => o.DestinationCity)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                // Update basic properties
                existingOrder.ClientName = order.ClientName;
                existingOrder.CargoType = order.CargoType;
                existingOrder.CargoWeight = order.CargoWeight;

                // Update city relationships if they've changed
                if (order.SourceCity != null)
                {
                    var sourceCity = await _context.Cities.FindAsync(order.SourceCity.id);
                    if (sourceCity != null)
                    {
                        existingOrder.SourceCity = sourceCity;
                    }
                }

                if (order.DestinationCity != null)
                {
                    var destCity = await _context.Cities.FindAsync(order.DestinationCity.id);
                    if (destCity != null)
                    {
                        existingOrder.DestinationCity = destCity;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var deliveries = await _context.Deliveries
                .Where(d => d.Order != null && d.Order.OrderId == orderId)
                .ToListAsync();
            if (deliveries.Any())
            {
                _context.Deliveries.RemoveRange(deliveries);
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
                _context.Orders.Remove(order);
            
            await _context.SaveChangesAsync();
        }
    }
}
