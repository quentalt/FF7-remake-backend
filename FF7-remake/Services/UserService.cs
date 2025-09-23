using FF7_remake.DBContext;
using FF7_remake.DTOs;
using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.Services;

public interface IUserService
{ 
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int id, string progress);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ValidateUserAsync(string email, string password);
        Task<UserDto?> AuthenticateAsync(LoginDto loginDto);
}
public class UserService : IUserService
{
        private readonly Ff7DbContext _context;

        public UserService(Ff7DbContext context)
        {
                _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
                return await _context.Users
                     .Select(u => new UserDto
                                {
                                UserId = u.UserId,
                                UserName = u.Username,
                                Email = u.Email,
                                Progress = u.Progress,
                                CreatedAt = u.CreatedAt
                        }).ToListAsync();
        }
        
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return null;

                return new UserDto
                {
                        UserId = user.UserId,
                        UserName = user.Username,
                        Email = user.Email,
                        Progress = user.Progress,
                        CreatedAt = user.CreatedAt
                };
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if(user == null) return null;

                return new UserDto
                {
                        UserId = user.UserId,
                        UserName = user.Username,
                        Email = user.Email,
                        Progress = user.Progress,
                        CreatedAt = user.CreatedAt
                };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
                var user = new User
                {
                        Username = createUserDto.UserName,
                        Email = createUserDto.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                        Progress = createUserDto.Progress,
                        CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new UserDto
                {
                        UserId = user.UserId,
                        UserName = user.Username,
                        Email = user.Email,
                        Progress = user.Progress,
                        CreatedAt = user.CreatedAt
                };
        }
        
        public async Task<UserDto?> UpdateUserAsync(int id, string progress)
        {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return null;

                user.Progress = progress;
                await _context.SaveChangesAsync();

                return new UserDto
                {
                        UserId = user.UserId,
                        UserName = user.Username,
                        Email = user.Email,
                        Progress = user.Progress,
                        CreatedAt = user.CreatedAt
                };
        }
        
        public async Task<bool> DeleteUserAsync(int id)
        {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
        }
        
        public async Task<bool> ValidateUserAsync(string email, string password)
        {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return false;

                return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
        
        public async Task<UserDto?> AuthenticateAsync(LoginDto loginDto)
        {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                        return null;
                }

                return new UserDto
                {
                        UserId = user.UserId,
                        UserName = user.Username,
                        Email = user.Email,
                        Progress = user.Progress,
                        CreatedAt = user.CreatedAt
                };
        }
}