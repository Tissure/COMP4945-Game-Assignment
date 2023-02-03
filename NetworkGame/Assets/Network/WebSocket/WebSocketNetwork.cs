using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using TMPro;

//using GameThread = _4945_A2.Threads.GameThread;
//using P = _4945_A2.Packet.Packet;

namespace NetworkModule
{
    public class WebSocketNetwork : Network
    {
        private HubConnection connection;

        private static WebSocketNetwork Instance = new WebSocketNetwork();

        private WebSocketNetwork() { }

        public static WebSocketNetwork GetWebSocketNetwork() { return Instance; }
        //public WebSocketNetwork(GameThread gt) : base(gt)
        //{
        //}

        //public WebSocketNetwork(int port, string ipAddress, GameThread gt) : base(port, ipAddress, gt)
        //{
        //}

        //public WebSocketNetwork(int port, string ipAddress, GameThread gt, int bufferSize) : base(port, ipAddress, gt, bufferSize)
        //{
        //}

        public override void Send(string packet)
        {
            Console.WriteLine("SEND: " + packet.ToString());
            byte[] b = ASCIIEncoding.ASCII.GetBytes(packet);
            connection.SendAsync("SendPacket", b);
        }

        public override void Setup()
        {
            connection = new HubConnectionBuilder().WithUrl("http://10.65.78.218:5000/gamehub").Build();
            Console.WriteLine("Connection " + connection.ToString());
            connection.StartAsync().Wait();
            Console.WriteLine(connection.State + " " + connection.ConnectionId);
        }

        protected override void Receive()
        {
            connection.On("RecievePacket", (byte[] packet) =>
            {
                PacketHandler.Packet p = new PacketHandler.Packet(packet);
                Console.WriteLine("RECIEVED: " + Encoding.ASCII.GetString(p.data));
            });
        }
    }
}

    