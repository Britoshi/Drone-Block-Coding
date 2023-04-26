using Brito;
using SharpDX.DirectInput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TelloConsole;
using TelloLib;
using UnityEngine;

public class DroneConnectionMaster : BritoBehavior
{
    public static DroneConnectionMaster Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //subscribe to Tello connection events
        Tello.onConnection += (Tello.ConnectionState newState) =>
        {
            if (newState != Tello.ConnectionState.Connected)
            {

            }
            if (newState == Tello.ConnectionState.Connected)
            {
                Tello.queryAttAngle();
                Tello.setMaxHeight(50);

                ClearConsole();
            }
            PrintAt(0, 0, "Tello " + newState.ToString());
        };

        //Log file setup.
        var logPath = "logs/";
        System.IO.Directory.CreateDirectory(Path.Combine("../", logPath));
        var logStartTime = DateTime.Now;
        var logFilePath = Path.Combine("../", logPath + logStartTime.ToString("yyyy-dd-M--HH-mm-ss") + ".csv");

        //write header for cols in log.
        //File.WriteAllText(logFilePath, "time," + Tello.state.getLogHeader());

        //subscribe to Tello update events.
        Tello.onUpdate += (cmdId) =>
        {
            if (cmdId == 86)//ac update
            {
                //write update to log.
                var elapsed = DateTime.Now - logStartTime;
                //File.AppendAllText(logFilePath, elapsed.ToString(@"mm\:ss\:ff\,") + Tello.state.getLogLine());

                //display state in console.
                var outStr = Tello.state.ToString();//ToString() = Formated state
                PrintAt(0, 2, outStr);

                //Now do the other stuff.
                Mission.Update();
            }
        };
        Tello.startConnecting();//Start trying to connect. 

    }


    //Print at x,y in console. 
    static void PrintAt(int x, int y, string str)
    {
        //var saveLeft = Console.CursorLeft;
        //var saveTop = Console.CursorTop;
        //Console.SetCursorPosition(x, y);
        //Console.WriteLine(str + "     ");//Hack. extra space is to clear any previous chars.
        //Console.SetCursorPosition(saveLeft, saveTop);

    }

    static void MyPrintAt(int x, int y, string str)
    {
        var saveLeft = Console.CursorLeft;
        var saveTop = Console.CursorTop;
        //print(SetCursorPosition(x, y));
        print(str + "     ");//Hack. extra space is to clear any previous chars.
        //Console.SetCursorPosition(saveLeft, saveTop);
    }

    static void ClearConsole()
    {
        //lmao
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // print(Tello.connected);
    }

    private void Update()
    {
        if (!Tello.connected)
        {
            print("Drone not connected yet!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Tello.Client.Send("motoron");
    }

    public static void PrintDroneNotConnected()
    {
        print("Drone is not connected!");
    }

    public static void SendCommands(List<string> commands)
    {
        if (!Tello.connected)
        {
            PrintDroneNotConnected();
            return;
        }

        Tello.takeOff();

        foreach (var command in commands)
        {
            Tello.Client.Send(command);
        }

        Tello.land();
    }

}
