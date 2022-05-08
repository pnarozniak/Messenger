using MessengerApi.Helpers.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace MessengerApi.Helpers.Providers
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User.GetId();
            return userId?.ToString();
        }
    }
}