using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerManager 
{
    PlayerControl GetPlayer();
    PlayerControl InstiantiatePlayer();

}