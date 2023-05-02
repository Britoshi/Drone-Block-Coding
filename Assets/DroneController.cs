using System.Collections;
using System.Collections.Generic; 
using UnityEngine; 
using TelloCommander.CommandDictionaries;
using TelloCommander.Commander;
using TelloCommander.Connections;
using TelloCommander.Interfaces;
using System.IO;
using System; 
using UnityEditor;
using System.Net;
using System.IO.Compression;
using System.Net.Sockets; 

public class DroneController : BritoBehavior
{
    public static DroneController Instance;

    static CommandDictionary dictionary;
    static DroneCommander commander;

    public static bool connected = false;

    public static string CONTENT_DIR => Application.persistentDataPath + "/Content";

    public delegate void VoidFunction();

    static List<VoidFunction> OnExecuteStart, OnExecuteEnd;

    public static void AddOnStartFunction(VoidFunction function) =>
        OnExecuteStart.Add(function);

    public static void AddOnEndFunction(VoidFunction function) =>
        OnExecuteEnd.Add(function);

    private void Awake()
    {
        if (!Directory.Exists(CONTENT_DIR) || Directory.GetFiles(CONTENT_DIR).Length <= 1)
        {
            string zipFileUrl = "https://raw.githubusercontent.com/Britoshi/Drone-Block-Coding/e12eeab0ba6308cf244b51afc85452aa7bed8343/Assets/Content.zip";
            string zipFileName = "Content.zip";

            Directory.CreateDirectory(CONTENT_DIR); 

            WebClient client = new();
            client.DownloadFile(zipFileUrl, zipFileName);

            ZipFile.ExtractToDirectory(zipFileName, CONTENT_DIR); 
        }

        Instance = this;
        connected = false;
        OnExecuteStart = new() { TakeOff };
        OnExecuteEnd = new() { Land };
    }

    public static void Crash()
    {

    }

    bool TestConnection()
    { 
        string ip = "192.168.10.1";
        int port = 8889;

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        var ipAdd = IPAddress.Parse(ip);
        var endpoint = new IPEndPoint(ipAdd, port);
        // Loop through all the local IP addresses
        foreach (IPAddress localIP in localIPs)
        {
            // Check if the local IP address is an IPv4 address

            for (int i = 0; i < 10; i++)
            {
                ip = ip.Substring(0, ip.Length - 1) + i.ToString();
                ipAdd = IPAddress.Parse(ip);
                endpoint = new IPEndPoint(ipAdd, port);

                if (localIP.Equals(ipAdd)) 
                { 
                    using UdpClient udpClient = new(new IPEndPoint(localIP, 0));
                    try
                    {
                        byte[] udpData = new byte[] { 0x01, 0x02, 0x03 };
                        udpClient.Send(udpData, udpData.Length, endpoint);
                        return true;
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        } 
        return false; 
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeTickFunction(DroneTickLoop);
    }

    void DroneTickLoop(object sender, OnTickEventArgs args)
    { 
        bool prevConnected = connected;
        connected = TestConnection();

        if (!connected && prevConnected)
            print("Drone lost connection.");
        else if (connected && !prevConnected)
        {
            print("Found connection! Trying to connect!");
            StartCoroutine(ConnectDrone());
        }
        else if (!connected && !prevConnected)
            print("Failed to connect to drone. Retrying in a second...");
    }

    IEnumerator ConnectDrone()
    {
        //yield return new WaitForSecondsRealtime(5f);
        InitializeDrone();
        yield return null;
    }

    void InitializeDrone()
    { 
        dictionary ??= CommandDictionary.ReadStandardDictionary("1.3.0.0");
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

    public static void RunCommands(List<string> commands)
    {

        foreach (var func in OnExecuteStart)
            func();
        Instance.StartCoroutine(Instance.Execute(commands));
    }

    void TakeOff() => 
        commander.RunCommand("takeoff");
    
    void Land() =>
        commander.RunCommand("land");

    IEnumerator Execute(List<string> commands)
    { 
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

        foreach (var func in OnExecuteEnd)
            func();
    }

}
