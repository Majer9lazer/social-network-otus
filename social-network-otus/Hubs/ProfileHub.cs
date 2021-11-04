using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace social_network_otus.Hubs
{
    public interface IProfileHubClient
    {
        Task ReceiveCreationProgress(int percentValue);
    }

    public class ProfileHub : Hub<IProfileHubClient>
    {
        public async Task SendCreationProgress(int percentValue, string userId)
        {
            await Clients.User(userId).ReceiveCreationProgress(percentValue);
        }
    }
}
