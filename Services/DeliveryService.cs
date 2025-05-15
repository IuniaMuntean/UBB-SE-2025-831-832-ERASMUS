using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;
using UBB_SE_2025_EUROTRUCKERS.Services.interfaces;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
   
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepository<Delivery> _deliveryRepository;
        private readonly ILoggingService _loggingService;

        public DeliveryService(
            IRepository<Delivery> deliveryRepository,
            ILoggingService loggingService)
        {
            _deliveryRepository = deliveryRepository;
            _loggingService = loggingService;
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.GetAllAsync();
            _loggingService.LogDebug($"Retrieved {deliveries?.Count() ?? 0} deliveries from repository");
            return deliveries ?? Enumerable.Empty<Delivery>();
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(int id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null)
            {
                _loggingService.LogWarning($"Delivery with ID {id} not found");
            }
            return delivery;
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int deliveryId, string status)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
            if (delivery != null)
            {
                delivery.status = status;
                await _deliveryRepository.UpdateAsync(delivery);
                _loggingService.LogInformation($"Updated delivery {deliveryId} status to {status}");
                return true;
            }
            _loggingService.LogWarning($"Failed to update delivery {deliveryId}: not found");
            return false;
        }

        public async Task<bool> CreateDeliveryAsync(Delivery delivery)
        {
            await _deliveryRepository.AddAsync(delivery);
            _loggingService.LogInformation($"Created new delivery with ID {delivery.delivery_id}");
            return true;
        }

        public async Task<bool> DeleteDeliveryAsync(int delivery_id)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(delivery_id);
            if (delivery != null)
            {
                await _deliveryRepository.DeleteAsync(delivery.delivery_id);
                _loggingService.LogInformation($"Deleted delivery {delivery_id}");
                return true;
            }
            _loggingService.LogWarning($"Failed to delete delivery {delivery_id}: not found");
            return false;
        }
    }
}
