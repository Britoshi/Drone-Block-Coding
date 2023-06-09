using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BritoBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected static void SubscribeTickFunction(EventHandler<OnTickEventArgs> func)
    {
        Tick.OnTick += func;
    }
    public static new void print(object message)
    {
        Sys.Log(message);
    }

    public static void error(object message)
    {
        Sys.LogError(message);
    }

    public static float abs(float value) =>
        Mathf.Abs(value);

    public static float sqrt(float value) =>
        Mathf.Sqrt(value);
}
