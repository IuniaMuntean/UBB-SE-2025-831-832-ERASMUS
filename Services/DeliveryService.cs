using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly TransportDbContext _context;
        private readonly ILoggingService _loggingService;

        public DeliveryService(
            TransportDbContext context,
            ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
            Debug.WriteLine("=== DeliveryService initialized ===");
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            Debug.WriteLine("=== Starting GetAllDeliveriesAsync ===");
            try
            {
                Debug.WriteLine("Checking if deliveries DbSet exists...");
                if (_context.Deliveries == null)
                {
                    Debug.WriteLine("ERROR: deliveries DbSet is null!");
                    return new List<Delivery>();
                }

                Debug.WriteLine("Attempting to query deliveries...");
                var query = _context.Deliveries
                    .Include(d => d.Order)
                 
                    .Include(d => d.Driver)
                    .Include(d => d.Truck);

                Debug.WriteLine("Executing query...");
                var deliveries = await query.ToListAsync();
                
                Debug.WriteLine($"Query completed. Found {deliveries?.Count ?? 0} deliveries");
                
                if (deliveries != null && deliveries.Any())
                {
                    foreach (var delivery in deliveries)
                    {
                        Debug.WriteLine($"\nDelivery ID: {delivery.DeliveryId}");
                        Debug.WriteLine($"- Order: {delivery.Order?.OrderId}");
                        Debug.WriteLine($"- Source City: {delivery.Order?.SourceCity?.name}");
                        Debug.WriteLine($"- Destination City: {delivery.Order?.DestinationCity?.name}");
                        Debug.WriteLine($"- Driver: {delivery.Driver?.DriverId}");
                        Debug.WriteLine($"- Truck: {delivery.Truck?.TruckId}");
                        Debug.WriteLine($"- Status: {delivery.Status}");
                    }
                }
                else
                {
                    Debug.WriteLine("No deliveries found in the database");
                }

                _loggingService.LogDebug($"Retrieved {deliveries?.Count ?? 0} deliveries from database");
                return deliveries;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR in GetAllDeliveriesAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                _loggingService.LogError("Error retrieving deliveries", ex);
                throw;
            }
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int id)
        {
            Debug.WriteLine($"=== Starting GetDeliveryByIdAsync for ID: {id} ===");
            try
            {
                var delivery = await _context.Deliveries
                    .Include(d => d.Order)
                    .Include(d => d.Order.SourceCity)
                    .Include(d => d.Order.DestinationCity)
                    .Include(d => d.Driver)
                    .Include(d => d.Truck)
                    .FirstOrDefaultAsync(d => d.DeliveryId == id);

                if (delivery == null)
                {
                    Debug.WriteLine($"No delivery found with ID: {id}");
                    _loggingService.LogWarning($"Delivery with ID {id} not found");
                }
                else
                {
                    Debug.WriteLine($"Found delivery {id}:");
                    Debug.WriteLine($"- Order: {delivery.Order?.OrderId}");
                    Debug.WriteLine($"- Status: {delivery.Status}");
                }
                return delivery;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR in GetDeliveryByIdAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int deliveryId, string status)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery != null)
            {
                delivery.Status = status;
                await _context.SaveChangesAsync();
                _loggingService.LogInformation($"Updated delivery {deliveryId} status to {status}");
                return true;
            }
            _loggingService.LogWarning($"Failed to update delivery {deliveryId}: not found");
            return false;
        }

        public async Task<bool> CreateDeliveryAsync(Delivery delivery)
        {
            // Ensure related entities exist
            if (delivery.Order != null)
            {
                var order = await _context.Orders
                    .Include(o => o.SourceCity)
                    .Include(o => o.DestinationCity)
                    .FirstOrDefaultAsync(o => o.OrderId == delivery.Order.OrderId);
                if (order != null)
                {
                    delivery.Order = order;
                }
            }

            if (delivery.Driver != null)
            {
                var driver = await _context.Drivers.FindAsync(delivery.Driver.DriverId);
                if (driver != null)
                {
                    delivery.Driver = driver;
                }
            }

            if (delivery.Truck != null)
            {
                var truck = await _context.Trucks.FindAsync(delivery.Truck.TruckId);
                if (truck != null)
                {
                    delivery.Truck = truck;
                }
            }

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
            _loggingService.LogInformation($"Created new delivery with ID {delivery.DeliveryId}");
            return true;
        }

        public async Task<bool> DeleteDeliveryAsync(int deliveryId)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                await _context.SaveChangesAsync();
                _loggingService.LogInformation($"Deleted delivery {deliveryId}");
                return true;
            }
            _loggingService.LogWarning($"Failed to delete delivery {deliveryId}: not found");
            return false;
        }
    }
}
