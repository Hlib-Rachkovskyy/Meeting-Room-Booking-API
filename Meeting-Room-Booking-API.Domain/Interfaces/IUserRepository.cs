using Meeting_Room_Booking_API.Domain.Entities;

namespace Meeting_Room_Booking_API.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user);
}
