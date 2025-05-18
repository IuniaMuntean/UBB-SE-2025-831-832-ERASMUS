using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UBB_SE_2025_EUROTRUCKERS.Data;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public class UserService : IUserService
    {
        private readonly TransportDbContext _db;

        public UserService(TransportDbContext db)
        {
            _db = db;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            try
            {
                Console.WriteLine("Checking if user exists...");

                if (await _db.Users.AnyAsync(u => u.Username == user.Username))
                {
                    Console.WriteLine("User already exists.");
                    return false;
                }

                Console.WriteLine("Hashing password...");
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                Console.WriteLine("Adding user...");
                _db.Users.Add(user);

                Console.WriteLine("Saving changes...");
                await _db.SaveChangesAsync();

                Console.WriteLine("User registered successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION: " + ex.Message);
                throw;
            }
        }

        public async Task<bool> LoginAsync(User user)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            return existingUser != null && BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);
        }
    }
} 