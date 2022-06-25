using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_MoveSpeed : EntityBuffBase
{
    public Buff_MoveSpeed(float pcntIndex)
    {
        buffChanger = new EntityDataChanger();
        buffChanger.moveSpeedPcnt = pcntIndex;
    }
}
