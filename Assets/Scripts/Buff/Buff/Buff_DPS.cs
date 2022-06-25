using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_DPS : EntityBuffBase
{
    public Buff_DPS(float pcntIndex)
    {
        buffChanger = new EntityDataChanger();
        buffChanger.DPSPcnt = pcntIndex;
    }
}
