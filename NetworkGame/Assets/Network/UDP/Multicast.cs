using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        // Initialize the multicast address group and multicast port.
        // Both address and port are selected from the allowed sets as
        // defined in the related RFC documents. These are the same 
        // as the values used by the sender.
        mcastAddress = IPAddress.Parse("230.0.0.1");
        mcastPort = 11000;

        try {
            // Create socket
            mcastSocket = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Dgram,
                                     ProtocolType.Udp);
            IPAddress localIP = IPAddress.Any;
            EndPoint localEP = (EndPoint) new IPEndPoint(localIP, mcastPort);
            mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

            // Bind socket
            mcastSocket.Bind(localEP);

            Thread receivingThread = new Thread(Receive);
            receivingThread.IsBackground = true;
            receivingThread.Start();

            // Create Thread to listen for incomming messages
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
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
        
        try
        {
            
            mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Hello Multicast Listener"), groupEP);
            Console.WriteLine("Multicast data sent.....");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.ToString());
            return false;
        }

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
