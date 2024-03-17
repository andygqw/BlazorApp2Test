using BlazorApp2Test.Data;
using BlazorApp2Test.Models;
using System;

namespace BlazorApp2Test.Components
{
    public class UserService
    {
        private readonly AuthService _service;

        private User? user = null;


        public UserService(AuthService service)
        {
            _service = service;
        }

        public async Task Register(RegisterModel model)
        {
            if(await _service.RegisterUser(model.Username, model.Password, model.Email))
            {
                user = await _service.AuthenticateUser(model.Username, model.Password);
            }
            else
            {
                throw new Exception("Create User failed");
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
    }
}
