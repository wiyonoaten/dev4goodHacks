using Microsoft.WindowsAzure.MobileServices;
using ServicesLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace ServicesLib
{
    public class PayPerPlaceServiceClient
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://dev4goodhacks.azure-mobile.net/",
            "gFJudrJYlUadKTorKdQvQEqUzFNgam89"
        );

        private IMobileServiceTable<User> userTable = MobileService.GetTable<User>();
        private IMobileServiceTable<Challenge> challengeTable = MobileService.GetTable<Challenge>();

        public PayPerPlaceServiceClient()
        {
        }

        public async Task<bool> CheckIsUserRegisteredAsync()
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //settings.Values.Clear();

            /*var users = from u in userTable
                        select u;
            foreach (var u in await users.ToEnumerableAsync())
            {
                await userTable.DeleteAsync(u);
            }*/
            
            return settings.Values.ContainsKey("UserId");
        }

        public async Task<bool> RegisterUserAsync(string name)
        {
            bool successful = false;

            try
            {
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                User newUser = new User
                {
                    Name = name,
                    GiveyTag = null,
                    PushChannelUri = channel.Uri,
                };

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

        public async Task<User> GetCurrentUserAsync()
        {
            User curUser = null;

            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (settings.Values.ContainsKey("UserId"))
            {
                int curUserId = (int)settings.Values["UserId"];

                /*var users = from u in userTable
                            where u.Id == curUserId
                            select u;

                var usersList = await users.ToListAsync();

                if ((usersList != null) && (usersList.Count > 0))
                {
                    curUser = usersList.First();
                }*/

                curUser = await userTable.LookupAsync(curUserId);
            }

            return curUser;
        }

        public async Task<bool> CreateNewChallengeAsync(Challenge challenge)
        {
            bool successful = false;

            try
            {
                await challengeTable.InsertAsync(challenge);

                successful = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return successful;
        }

        public async Task<List<Challenge>> GetRunningChallengesAsync()
        {
            List<Challenge> list = null;

            try
            {
                var challenges = await challengeTable.ToListAsync();

                list = challenges;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return list;
        }
    }
}
