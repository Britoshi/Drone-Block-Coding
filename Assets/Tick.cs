using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTickEventArgs : EventArgs
{
    public int tick;
} 

public class Tick : BritoBehavior
{
    protected static Tick Instance; 

    public static event EventHandler<OnTickEventArgs> OnTick; 

    private const float TICK_MILISECOND = 1000; 

    private float TICK_TIMER_MAX => TICK_MILISECOND / 1000f;

    public static float deltaTime => Instance.TICK_TIMER_MAX; 

    int tick;
    float tickTimer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tick = 0; 
    }

    // Update is called once per frame
    void Update()
    {

        tickTimer += Time.deltaTime;
        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;

            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });  

        }
    }  
}

public delegate void TickFunction(object sender, OnTickEventArgs e);
 