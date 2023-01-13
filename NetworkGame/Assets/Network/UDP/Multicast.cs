using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class Multicast {
    private static IPAddress mcastAddress;
    private static int mcastPort;
    private static Socket mcastSocket;
    private static MulticastOption mcastOption;
    private static IPAddress localIP;
    private static EndPoint localEP;
    private static IPEndPoint groupEP;
    private static IPEndPoint remoteEP;

    /// <summary>
    /// Initializes network with default IP and port number
    /// </summary>
    public void initDefaultNetwork() {
        // Create endpoint
        // Create socket
        // Bind socket
        // Create Thread to listen for incomming messages
    }

    /// <summary>
    /// Sends Yeet over socket connection
    /// </summary>
    /// <returns>
    /// <c>True</c> if messgae successfully sends.
    /// <c>False</c> otherwise
    /// </returns>
    public bool Send() {
        //if (socket == null) {
        //    System.Diagnostics.Debug.WriteLine("Network not initialized");
        //    return false;
        //}
        //string msg = "Yeet";
        //System.Diagnostics.Debug.WriteLine("Message sent: " + msg);
        //return true;
    }

    /// <summary>
    /// Recieves Payload from Socket Connection Subscribed to Sender. 
    /// Delegate method: To be run in a background thread, to 'recieve' constantly.
    /// </summary>
    public void Recieve()
    {

        // Create new Socket, defining the AddressFamily, SocketType, and ProtocolType
        mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Multicast Address that the reciever will 'subscribe' to
        mcastAddress = IPAddress.Parse("230.0.0.1");

        // Multicast Port
        mcastPort = 11000;
        // Set local IPaddress to Any
        localIP = IPAddress.Any;

        // Endpoint to Bind to
        localEP = new EndPoint(localIP, mcastPort);
        mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
        
        // Bind to IPEndpoint
        mcastSocket.Bind(localEP);

        // Create Multicast Option to set later
        mcastOption = new MulticastOption(mcastAddress, localIP);

        // Set socket options. Subscribe (Add Membership) to Multicast Broadcast
        mcastOption.SetSocketOption(SocketOptionLevel.IP, SocketOptionLevelName.AddMembership, mcastOption);

        byte[] message = new byte[1024];

        // Endpoint to recieve message from (Any Ip Address, Port: 0)
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

        // Recieved bytes
        int recv

        while (true) 
        {
            Console.WriteLine("Waiting for packets..");
            recv = mcastSocket.ReceiveFrom(message, ref remoteEP)
            Console.WriteLine("Recieved packets..\n" + Encoding.ASCII.GetString(message));
        }
        // Release Connection and resources
        mcastSocket.Close();
    }

}
