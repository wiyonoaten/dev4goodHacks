using Microsoft.WindowsAzure.MobileServices;
using ServicesLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLib
{
    public class PayPerPlaceServiceClient
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://dev4goodhacks.azure-mobile.net/",
            "gFJudrJYlUadKTorKdQvQEqUzFNgam89"
        );

        private IMobileServiceTable<User> userTable = MobileService.GetTable<User>();

        public PayPerPlaceServiceClient()
        {
        }

        public bool CheckIsUserRegistered()
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            
            return settings.Values.ContainsKey("UserId");
        }

        public async Task<bool> RegisterUserAsync(string name)
        {
            bool successful = false;

            User newUser = new User
                {
                    Name = name,
                    CharityId = 0,
                    GiveyTag = "",
                };

            try
            {
                await userTable.InsertAsync(newUser);

                var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                settings.Values["UserId"] = newUser.Id; // remember the user

                successful = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return successful;
        }
    }
}
