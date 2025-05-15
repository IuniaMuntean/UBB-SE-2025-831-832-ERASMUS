using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services.interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<Delivery>> GetAllDeliveriesAsync();
        Task<Delivery?> GetDeliveryByIdAsync(int id);
        Task<bool> UpdateDeliveryStatusAsync(int deliveryId, string status);
        Task<bool> CreateDeliveryAsync(Delivery delivery);
        Task<bool> DeleteDeliveryAsync(int deliveryId);
    }
}
