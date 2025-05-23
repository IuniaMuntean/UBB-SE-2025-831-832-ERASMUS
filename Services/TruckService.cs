using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public class TruckService : ITruckService
    {
        private readonly TransportDbContext _context;
        private readonly ILoggingService _loggingService;

        public TruckService(TransportDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        public async Task<IEnumerable<Truck>> GetAllAsync()
        {
            try
            {
                return await _context.Trucks.ToListAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting trucks: {ex.Message}", ex);
                throw;
            }
        }
    }
} 