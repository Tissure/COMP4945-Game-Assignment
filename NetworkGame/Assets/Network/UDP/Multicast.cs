using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace NetworkModule {
    public class Multicast : CustomNetworkModule
    {
        private static IPAddress mcastAddress;
        private static int mcastPort;
        private static Socket mcastSocket;
        private static MulticastOption mcastOption;
        private static IPAddress localIP;
        private static EndPoint localEP;
        private static IPEndPoint groupEP;
        private static EndPoint remoteEP;

        /// <summary>
        /// Initializes network with default IP and port number
        /// </summary>
        public void initDefaultNetwork() {
            // Create endpoint

            // Initialize the multicast address group and multicast port.
            // Both address and port are selected from the allowed sets as
            // defined in the related RFC documents. These are the same 
            // as the values used by the sender.

            // Multicast Address that the reciever will 'subscribe' to
            mcastAddress = IPAddress.Parse("230.0.0.1");
            

            // Multicast Port
            mcastPort = 11000;

            try {
                // Set local IPaddress to Any THIS SHOULD WORK BUT DOESNT
                //localIP = IPAddress.Any;

                // Set localIp to current machine address
                string ip = GetLocalIPv4(NetworkInterfaceType.Wireless80211);
                localIP = IPAddress.Parse(ip);
                Debug.Log(localIP.ToString());

                // Create new Socket, defining the AddressFamily, SocketType, and ProtocolType
                mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                mcastSocket.EnableBroadcast = true;

                // Endpoint to Bind to
                localEP = (EndPoint) new IPEndPoint(localIP, mcastPort);
                // Endpoint to send to
                groupEP = new IPEndPoint(mcastAddress, mcastPort);

                Debug.Log(groupEP.ToString());

                // Set socket option to reuse address
                mcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

                // Bind to IPEndpoint
                mcastSocket.Bind(localEP);

                Thread receivingThread = new Thread(new ThreadStart(Receive));
                receivingThread.IsBackground = true;
                receivingThread.Start();

                // Create Thread to listen for incomming messages
            } catch (Exception e) {
                Debug.Log(e.ToString());
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

            try {

                //mcastSocket.SendTo(ASCIIEncoding.ASCII.GetBytes("Hello Multicast Listener"), groupEP);
                // Testing PacketBuilder
                Packet packet = new Packet();
                byte[] msg = ASCIIEncoding.ASCII.GetBytes(packet.serverBuildPacket(1, 2, 3));
                mcastSocket.SendTo(msg, 0, msg.Length, SocketFlags.None, groupEP);
                 Debug.Log("Multicast data sent.....");
                return true;
            } catch (Exception e) {
                Debug.Log("\n" + e.ToString());
                return false;
            }

        }

        internal static string GetLocalIPv4(NetworkInterfaceType _type) {  // Checks your IP adress from the local network connected to a gateway. This to avoid issues with double network cards
            string output = "";  // default output
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) // Iterate over each network interface
            {  // Find the network interface which has been provided in the arguments, break the loop if found
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up) {   // Fetch the properties of this adapter
                    IPInterfaceProperties adapterProperties = item.GetIPProperties();
                    // Check if the gateway adress exist, if not its most likley a virtual network or smth
                    if (adapterProperties.GatewayAddresses.FirstOrDefault() != null) {   // Iterate over each available unicast adresses
                        foreach (UnicastIPAddressInformation ip in adapterProperties.UnicastAddresses) {   // If the IP is a local IPv4 adress
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {   // we got a match!
                                output = ip.Address.ToString();
                                break;  // break the loop!!
                            }
                        }
                    }
                }
                // Check if we got a result if so break this method
                if (output != "") {
                    break;
                }
            }
            // Return results
            return output;
        }

        /// <summary>
        /// Recieves Payload from Socket Connection Subscribed to Sender. 
        /// Delegate method: To be run in a background thread, to 'receive' constantly.
        /// </summary>
        public void Receive() {
            // Create Multicast Option to set later
            mcastOption = new MulticastOption(mcastAddress, localIP);

            // Set socket options. Subscribe (Add Membership) to Multicast Broadcast
            mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);

            byte[] message = new byte[1024];

            // Endpoint to recieve message from (Any Ip Address, Port: 0)
            remoteEP = new IPEndPoint(IPAddress.Any, 0);

            Debug.Log("Waiting for packets..");
            // Recieved bytes
            int recv = mcastSocket.ReceiveFrom(message, ref remoteEP);

            Packet packet = new Packet();

            while (recv != 0) {
                Debug.Log("Recieved packets..\n" + packet.readPacket(Encoding.ASCII.GetString(message)));
                Debug.Log("Recieved Packets.. \n" + Encoding.ASCII.GetString(message));
                recv = mcastSocket.ReceiveFrom(message, ref remoteEP);
            }
        }

    }
}
