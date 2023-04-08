using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ConnectionMaster : BritoBehavior
{

    bool listeningForCommand = false;
    bool listeningForState = false;

    byte[] commandData, stateData;
    EndPoint CommandEndPoint, StateEndPoint;


    public static IPAddress IP { private set; get; } 
    
    public static Socket CommandSocket { private set; get; } 
    public static Socket StateSocket { private set; get; } 



    public const int BUFFER_SIZE = 1024;
    public const short COMMAND_PORT = 8889,
                        SENSOR_PORT = 8890,
                        STREAM_PORT = 11111; 

    void Awake()
    { 
        CommandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        StateSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        commandData = new byte[BUFFER_SIZE];
        stateData = new byte[BUFFER_SIZE];


        AssignValidIPAddress(); 
        print("Connecting to drone! " + IP);

        Connect(CommandSocket, IP, COMMAND_PORT, out CommandEndPoint);
        Connect(StateSocket, IP, SENSOR_PORT, out StateEndPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        


    } 

    List<IPAddress> GetAvailableIPAddresses()
    {
        List<IPAddress> availableConnections = new List<IPAddress>(); 
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        { 
            if (ip.AddressFamily == AddressFamily.InterNetwork) 
                availableConnections.Add(ip); 
        }
        return availableConnections;
    }

    void AssignValidIPAddress()
    {
        foreach (var ip in GetAvailableIPAddresses())
            if(ip.ToString().Contains("192.168.10")) 
            {
                IP = ip;
                return;
            } 

        IP = null;
        error("Error: Invalid IP address or Failed to connect to drone");
    } 

    void Connect(Socket socket, IPAddress ip, short port, out EndPoint endpoint)
    {  
        var commandIPEndPoint = new IPEndPoint(ip, port);
        //CommandSocket.Bind(commandIPEndPoint);

        print("Binding Complete!");
        endpoint = commandIPEndPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!listeningForCommand)
            StartCoroutine(HandleSocket(CommandSocket, CommandEndPoint, commandData, OnReceiveCommand, true));
        
        if(!listeningForState)
            StartCoroutine(HandleSocket(StateSocket, StateEndPoint, stateData, OnReceiveState, false));
    }

    void OnReceiveCommand()
    {
        byte[] buffer = commandData;
        print($"Data received from drone at command socket");
        if(buffer == null) 
        {
            print("There was an error, ignoring processing command...");
            return;
        }

        Dictionary<string, string> states = new(); 

        //buffer.  
        string data = System.Text.Encoding.ASCII.GetString(buffer);

        data = data.Trim();
        print(data);
    }

    void OnReceiveState()
    { 
        byte[] buffer = stateData;
        print($"Data received from drone at state socket");
        if(buffer == null) 
        {
            print("There was an error, ignoring processing state...");
            return;
        } 
        
        Dictionary<string, string> states = new(); 
        
        //buffer.  
        string data = System.Text.Encoding.ASCII.GetString(buffer);

        data = data.Trim();
        print(data);
    }

    public delegate void OnReceiveResponse();

    IEnumerator HandleSocket(Socket socket, EndPoint endPoint, byte[] buffer, OnReceiveResponse function,
    bool commandSocket)
    {
        bool CommandReceived()
        {    
            try
            {
                SocketAsyncEventArgs socketEventArgs = new SocketAsyncEventArgs();
                socketEventArgs.SetBuffer(buffer, 0, BUFFER_SIZE);  

                socketEventArgs.RemoteEndPoint = endPoint;
                return CommandSocket.ReceiveFromAsync(socketEventArgs); 
            }
            catch (Exception exception) 
            {
                error(exception); 
                buffer = null;
                return true;
            }
        }

        buffer = new byte[BUFFER_SIZE];
        if(commandSocket) listeningForCommand = true;
        else listeningForState = true;

        yield return new WaitUntil(CommandReceived);

        function();

        if(commandSocket) listeningForCommand = false;
        else listeningForState = false;
    }
}
