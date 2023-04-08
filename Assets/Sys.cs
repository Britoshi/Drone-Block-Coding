using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sys
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Log(object message)
    {
        Debug.Log(message);
    }

    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
}
