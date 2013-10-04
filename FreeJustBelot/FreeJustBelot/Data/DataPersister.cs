using FreeJustBelot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeJustBelot.Data
{
    public class DataPersister
    {
        private const string BaseUrl = "http://freejustbelot.apphb.com/api/";
        private static HttpRequester requester = new HttpRequester(BaseUrl);

        public static string GetBaseUrl()
        {
            return BaseUrl;
        }

        public async static Task<LoginModelReceived> RegisterUser(UserModel registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("Model is null");
            }

            if (string.IsNullOrWhiteSpace(registration.Username))
            {
                throw new ArgumentException("Invalid username");
            }

            if (string.IsNullOrWhiteSpace(registration.Nickname))
            {
                throw new ArgumentException("Invalid nickname");
            }

            if (string.IsNullOrWhiteSpace(registration.AuthCode))
            {
                throw new ArgumentException("Invalid authcode");
            }

            string serviceUrl = "users/register";

            var response = await requester.PostAsync<LoginModelReceived>(serviceUrl, registration);

            return response;
        }

        public async static Task<LoginModelReceived> LoginUser(UserModel login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("Model is null");
            }

            if (string.IsNullOrWhiteSpace(login.Username))
            {
                throw new ArgumentException("Invalid username");
            }

            if (string.IsNullOrWhiteSpace(login.AuthCode))
            {
                throw new ArgumentException("Invalid authcode");
            }

            string serviceUrl = "users/login";

            var response = await requester.PostAsync<LoginModelReceived>(serviceUrl, login);

            return response;
        }

        public async static Task<object> LogoutUser(string sessionKey)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
            {
                throw new ArgumentNullException("Session key is null.");
            }

            LogoutModel logout = new LogoutModel { SessionKey = sessionKey };

            string serviceUrl = "users/logout";

            var response = await requester.PutAsync<object>(serviceUrl, logout);

            return response;
        }

        public async static Task<IEnumerable<GameModel>> GetAllGames(string sessionKey)
        {
            string serviceUrl = "games/all?sessionKey=" + sessionKey;
            return await requester.CreateGetRequestAsync<IEnumerable<GameModel>>(serviceUrl);
        }

        public async static Task<MessageModel> CreateGameAsync(CreateGameModel gameTobeCreated, string sessionKey)
        {
            string serviceUrl = "games/create?sessionKey=" + sessionKey;
            return await requester.PostAsync<MessageModel>(serviceUrl, gameTobeCreated);
        }

        public async static Task<MessageModel> JoinGameAsync(JoinGameModel gameTobeJoined, string sessionKey)
        {
            string serviceUrl = "games/join?sessionKey=" + sessionKey;
            return await requester.PostAsync<MessageModel>(serviceUrl, gameTobeJoined);
        }

        public async static Task<MessageModel> LeaveGameAsync(string gameName, string sessionKey)
        {
            string serviceUrl = "games/leave?sessionKey=" + sessionKey + "&gameName=" + gameName;
            return await requester.CreateGetRequestAsync<MessageModel>(serviceUrl);
        }

        public async static Task<IEnumerable<string>> GetFriendsStatuses(string sessionKey)
        {
            string serviceUrl = "friends/get-online?sessionKey=" + sessionKey;
            return await requester.CreateGetRequestAsync<IEnumerable<string>>(serviceUrl);
        }

        public async static Task<MessageModel> FindFriend(string sessionKey, string friendNickName)
        {
            string serviceUrl = "friends/find?sessionKey=" + sessionKey;
            FriendModel model = new FriendModel
            {
                FriendName = friendNickName
            };
            return await requester.PostAsync<MessageModel>(serviceUrl, model);
        }
    }
}
