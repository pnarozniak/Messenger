using MessengerApi.Dtos.Chat;
using MessengerApi.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessengerApi.Hubs
{
    public interface INotificationsHub
    {
        [HubMethodName("NewMessage")]
        public Task NewMessageAsync(int idChat, SingleMessageDto newMessage);

        [HubMethodName("MessageRemoved")]
        public Task MessageRemovedAsync(int idChat, int idMessage);
    }

    [Authorize]
    public class NotificationsHub : Hub<INotificationsHub>
    {
        
    }
}