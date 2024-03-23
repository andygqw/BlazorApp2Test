using BlazorApp2Test.Data;
using BlazorApp2Test.Models;
using System;

namespace BlazorApp2Test.Components
{
    public class UserService
    {
        private readonly AuthService _service;
        private readonly UserDbContext _context;


        private User? user = null;

        public UserService(AuthService service, UserDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task Register(RegisterModel model)
        {
            if(model.Username != null 
                && model.Password != null 
                && model.Email != null)
            {
                if (await _service.RegisterUser(model.Username, model.Password, model.Email))
                {
                    user = await _service.AuthenticateUser(model.Username, model.Password);
                }
                else
                {
                    throw new Exception("Create User failed");
                }
            }
        }

        public async Task Login(string username, string password)
        {
            try
            {
                user = await _service.AuthenticateUser(username, password);
            }catch(Exception ex)
            {
                throw new Exception("User Auth: " + ex.Message);
            }
        }

        public void Logout()
        {
            user = null;
        }

        public bool GetAuth()
        {
            if(user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetUsername()
        {
            if(user != null)
            {
                return user.username;
            }
            else
            {
                throw new Exception("Not log in yet");
            }
        }

        public async Task<string> GetUsername(int id)
        {
            try
            {
                var u = await _service.GetUserById(id);
                return u.username;

            }catch(Exception ex)
            {
                throw new Exception("UserService: " + ex.Message);
            }
        }

        public int GetUserId()
        {
            if(user != null)
            {
                return user.id;
            }
            else
            {
                throw new Exception("Not log in yet");
            }
        }

        public async Task UpdateUsername(int id, string name)
        {
            if(_context.Users != null)
            {
                var user  = await _context.Users.FindAsync(id);

                if(user != null)
                {
                    user.username = name;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("UpdateUser: Can't find this user");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users");
            }
        }

        public async Task UpdateEmail(int id, string email)
        {
            if (_context.Users != null)
            {
                var user = await _context.Users.FindAsync(id);

                if (user != null)
                {
                    user.email = email;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("UpdateUser: Can't find this user");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users");
            }
        }

        public async Task UpdatePassword(int id, string password)
        {
            if (_context.Users != null)
            {
                var user = await _context.Users.FindAsync(id);

                if (user != null)
                {
                    user.password = _service.HashPassword(password);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("UpdateUser: Can't find this user");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users");
            }
        }
    }
}