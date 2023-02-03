using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Threading.Tasks;


namespace SignalRChat.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendPacket(byte[] packet)
        {
            string temp = Encoding.ASCII.GetString(packet);
            Console.WriteLine("PACKET RECIEVED: " + temp);
            await Clients.All.SendAsync("RecievePacket", packet);
        }
    }
}