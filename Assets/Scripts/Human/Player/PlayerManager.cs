using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerManager : LogicModuleBase,IPlayerManager
{
    private PlayerControl player;
    private AimManager aim;
    public override void Init()
    {
        base.Init();
        
    }
    public PlayerControl GetPlayer()
    {
        return player;
    }

    public PlayerControl InstiantiatePlayer()
    {
        GameObject p = ResourceManager.Instance.Instantiate("Prefabs/Player/Player");
        player = p.GetComponent<PlayerControl>();
        player.Init();
        return player;
    }
}
