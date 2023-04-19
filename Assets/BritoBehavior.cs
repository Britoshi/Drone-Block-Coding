using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
