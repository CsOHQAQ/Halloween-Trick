using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_MaxHealth : EntityBuffBase
{
    public Buff_MaxHealth(float pcntIndex)
    {
        buffChanger = new EntityDataChanger();
        buffChanger.maxHealthPcnt = pcntIndex;
    }
}
