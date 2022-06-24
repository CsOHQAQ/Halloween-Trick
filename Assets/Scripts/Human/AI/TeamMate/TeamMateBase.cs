using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMateBase : AiBase
{
    //亲密度
    [HideInInspector]
    public float Intimacy;

    public bool AutoFire = false; //自动射击 开启后将自动攻击视野内的敌人
}
