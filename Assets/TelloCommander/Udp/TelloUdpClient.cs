using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TelloCommander.Udp
{
    [ExcludeFromCodeCoverage]
    public class TelloUdpClient : TelloUdpConnection
    {
        public int connectionAttempts = 0;

        public void Connect(IPAddress remoteAddress, int port)
        {
            _sendEndpoint = new IPEndPoint(remoteAddress, port);
            _receiveEndpoint = new IPEndPoint(IPAddress.Any, 0);
            _client = new UdpClient();
            _client.Connect(_sendEndpoint);
        }

        void TryConnect()
        {
            if(connectionAttempts >= 5)
            {
                BritoBehavior.error("Failed To Connect to drone after 5 tries! Crashing!!!");
                return;
            } 
            Task task = Task.Run(() => _client.Connect(_sendEndpoint));
            if (task.Wait(TimeSpan.FromSeconds(5))) // Wait for 5 seconds
            {
                UnityEngine.Debug.Log("Connection Successful!");
                return;
            } 
            connectionAttempts++; 
            TryConnect();
        }

        public void ConnectAsync(IPAddress remoteAddress, int port)
        { 
            _sendEndpoint = new IPEndPoint(remoteAddress, port);
            _receiveEndpoint = new IPEndPoint(IPAddress.Any, 0);
            _client = new UdpClient();
            connectionAttempts = 0;
            TryConnect();
        }
    }
}
