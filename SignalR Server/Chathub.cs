using Microsoft.AspNetCore.SignalR;

namespace SignalR_Server
{
    public class Chathub : Hub
    {
        public async Task SendMessage(string message, string name)
        {
            Console.WriteLine($"Received message:{name} says: {message}");
            await Clients.All.SendAsync("MessageReceived",name, message);
        }
    }
}
