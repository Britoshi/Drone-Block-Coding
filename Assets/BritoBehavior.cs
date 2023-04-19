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

    public new void print(object message)
    {
        Sys.Log(message);
    }

    public void error(object message)
    {
        Sys.LogError(message);
    }

    public float abs(float value) =>
        Mathf.Abs(value);

    public float sqrt(float value) =>
        Mathf.Sqrt(value);
}
