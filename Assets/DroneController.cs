using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

using TelloCommander.CommandDictionaries;
using TelloCommander.Commander;
using TelloCommander.Connections;
using TelloCommander.Interfaces;
using System.IO;
using System;
using UnityEditor.VersionControl;
using UnityEditor;
using System.Net;
using System.IO.Compression;
using System.Net.Sockets;
using UnityEditor.Experimental.GraphView;

public class DroneController : BritoBehavior
{
    public static DroneController Instance;

    static CommandDictionary dictionary;
    static DroneCommander commander;

    public static string CONTENT_DIR => Application.persistentDataPath + "/Content"; 

    private void Awake()
    {
        if (!Directory.Exists(CONTENT_DIR))
        {
            string zipFileUrl = "https://raw.githubusercontent.com/Britoshi/Drone-Block-Coding/e12eeab0ba6308cf244b51afc85452aa7bed8343/Assets/Content.zip";
            string zipFileName = "Content.zip";

            Directory.CreateDirectory(CONTENT_DIR); 

            WebClient client = new();
            client.DownloadFile(zipFileUrl, zipFileName);

            ZipFile.ExtractToDirectory(zipFileName, CONTENT_DIR); 
        }

        Instance = this;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        bool failCheck = false;
        string ip = "192.168.10.2";
        int port = 8889;

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        var ipAdd = IPAddress.Parse(ip);
        var endpoint = new IPEndPoint(ipAdd, port);
        // Loop through all the local IP addresses
        foreach (IPAddress localIP in localIPs)
        {
            // Check if the local IP address is an IPv4 address
            if (localIP.Equals(ipAdd))
            //if (localIP.AddressFamily == AddressFamily.InterNetwork)
            {
                // Create a UDP client and bind it to the local IP address and any available port
                using UdpClient udpClient = new(new IPEndPoint(localIP, 0));
                try
                {
                    // Create a UDP packet to send to the specified IP and port
                    byte[] udpData = new byte[] { 0x01, 0x02, 0x03 };
                    udpClient.Send(udpData, udpData.Length, endpoint);

                    print($"Sent UDP packet to {ip}:{port} from {localIP}:{((IPEndPoint)udpClient.Client.LocalEndPoint).Port}");
                    failCheck = true;
                }
                catch (Exception ex)
                {
                    print($"Error sending UDP packet: {ex.Message}");
                    return;
                }
            }
        }

        if (!failCheck) return;

        ConnectDrone();
    }

    void ConnectDrone()
    {

        dictionary = CommandDictionary.ReadStandardDictionary("1.3.0.0");
        print("Imported Dictionary");

        commander = new DroneCommander(new TelloConnection(), dictionary);
        print("Initiated Drone Commander");  

        commander.Connect();
        print("Connection Started!");
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public static void RunCommands(List<string> commands) =>
        Instance.StartCoroutine(Instance.Execute(commands));

    IEnumerator Execute(List<string> commands)
    { 
        commander.RunCommand("takeoff");

        yield return new WaitForSecondsRealtime(.1f);

        foreach (var command in commands)
        {
            try
            {
                commander.RunCommand(command);
            }
            catch
            {
                break;
            }
            yield return new WaitForSecondsRealtime(.1f);
        }
         
        yield return new WaitForSecondsRealtime(.1f);

        commander.RunCommand("land");
    }

}
