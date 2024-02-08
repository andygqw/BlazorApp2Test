
using BlazorApp2Test.Models;
using System.Text.Json;

namespace BlazorApp2Test.Components
{
    public class PasswordService
    {
        private bool isAllowed;
        private string userName;

        public PasswordService() 
        {
            isAllowed = false;
            userName = string.Empty;
        }

        internal async Task CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password not entered");
            }

            if(File.Exists(Helper.PasswordPath))
            {
                var str = await File.ReadAllTextAsync(Helper.PasswordPath);

                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new Exception("Password not set");
                }

                var results = JsonSerializer.Deserialize<List<PasswordModel>>(str);

                if (results != null && results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        if (result.Password == password)
                        {
                            if (result.Username == null || result.Username.Trim().Length == 0)
                            {
                                throw new Exception("Username not set");
                            }
                            isAllowed = true;
                            userName = result.Username;
                            return;
                        }
                    }

                    throw new Exception("Incorrect password");
                }
                else
                {
                    throw new Exception("Password not set correctly");
                }
            }
            else
            {
                throw new Exception("Can't find password");
            }
        }

        internal bool GetAuth()
        {
            return isAllowed;
        }

        internal string GetUserName()
        {
            return userName;
        }
    }
}