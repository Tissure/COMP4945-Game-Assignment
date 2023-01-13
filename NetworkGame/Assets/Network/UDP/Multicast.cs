using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class Multicast {
    Socket socket = null;
    IPEndPoint IPEndPoint = null;

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
}
