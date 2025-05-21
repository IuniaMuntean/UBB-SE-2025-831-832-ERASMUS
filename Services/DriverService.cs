using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public class DriverService : IDriverService
    {
        private readonly TransportDbContext _context;
        private readonly ILoggingService _loggingService;

        public DriverService(TransportDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            try
            {
                return await _context.Drivers.ToListAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError($"Error getting drivers: {ex.Message}", ex);
                throw;
            }
        }
    }
} 