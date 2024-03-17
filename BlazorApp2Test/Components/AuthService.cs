using BlazorApp2Test.Models;
using BlazorApp2Test.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Text;

namespace BlazorApp2Test.Components
{
    public class AuthService
    {
        private readonly UserDbContext _context;


        public AuthService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUser(string uname, string pwd, string eml)
        {
            if(string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(eml))
            {
                throw new ArgumentException("Username, password, and email are required to have value");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == uname || u.email == eml);
            if (user != null)
            {
                throw new Exception("Username or email already in-use");
            }

            var hashedPassword = HashPassword(pwd);
            user = new User { username = uname, password = hashedPassword, email = eml };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Username and password must not be empty");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user != null && VerifyPassword(password, user.password))
            {
                return user;
            }
            else
            {
                throw new Exception("Username and password didn't match");
            }
        }

        internal async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);

            if(user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("Can't find user with this " + id);
            }
        }

        private string HashPassword(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            return passwordHash;
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}