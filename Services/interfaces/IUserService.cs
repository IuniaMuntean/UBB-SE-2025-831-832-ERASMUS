using System.Threading.Tasks;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user);
        Task<bool> LoginAsync(User user);
    }
} 