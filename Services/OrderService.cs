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

        public async Task<List<Order>> GetOrdersAsync() => await _context.orders.ToListAsync();

        public async Task AddOrderAsync(Order order)
        {
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.orders.FindAsync(orderId);
            if (order != null)
            {
                _context.orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
