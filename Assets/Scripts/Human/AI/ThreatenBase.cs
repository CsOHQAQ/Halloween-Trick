using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ThreatenBase
{
    public float Threaten; //威胁
    public float AlertTime; //警戒时间，在此时间降为0之前Threaten不会下降

    public ThreatenBase(float threaten, float alertTime, AiBase self)
    {
        Threaten = threaten * self.arg1;
        AlertTime = alertTime * self.arg2;
    }
}