using BlazorApp2Test.Data;
using BlazorApp2Test.Models;
using System;
using System.Xml.Linq;

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
            if (model.Username != null
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
            }
            catch (Exception ex)
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
            if (user == null)
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
            if (user != null)
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

            }
            catch (Exception ex)
            {
                throw new Exception("UserService: " + ex.Message);
            }
        }

        public int GetUserId()
        {
            if (user != null)
            {
                return user.id;
            }
            else
            {
                throw new Exception("Not log in yet");
            }
        }

        public async Task UpdateUsername(string name)
        {
            if (_context.Users != null)
            {
                if (user != null)
                {
                    var u = await _context.Users.FindAsync(user.id);

                    if (u != null)
                    {
                        u.username = name;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("UpdateUser: Can't find this user");
                    }
                }
                else
                {
                    throw new Exception("Not Logged in");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users or not logged in");
            }
        }

        public async Task UpdateEmail(string email)
        {
            if (_context.Users != null)
            {
                if (user != null)
                {
                    var u = await _context.Users.FindAsync(user.id);

                    if (u != null)
                    {
                        u.email = email;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("UpdateUser: Can't find this user");
                    }
                }
                else
                {
                    throw new Exception("Not Logged in");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users or not logged in");
            }
        }

        public async Task UpdatePassword(string password)
        {
            if (_context.Users != null)
            {
                if (user != null)
                {
                    var u = await _context.Users.FindAsync(user.id);

                    if (u != null)
                    {
                        u.password = _service.HashPassword(password);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("UpdateUser: Can't find this user");
                    }
                }
                else
                {
                    throw new Exception("Not Logged in");
                }
            }
            else
            {
                throw new Exception("Something wrong with db Users or not logged in");
            }
        }
    }
}