using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using QxFramework.Utilities;
using App.Common;
using UnityEngine.UI;

public class EntityManager : MonoSingleton<EntityManager>
{
    public PlayerEntity player;

    public void Init()
    {
        player = new PlayerEntity();
        player = ResourceManager.Instance.Instantiate("Prefabs/TestPlayer").GetComponent<PlayerEntity>();
    }

    private void Update()
    {
        
    }



}
