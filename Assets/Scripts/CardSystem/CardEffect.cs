using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 此处纯存放对玩家的加强卡牌的使用效果
/// </summary>
public class CardEffect 
{
    public static void BaseSpread(float SpreadShrink)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_BaseSpread(SpreadShrink),9999999);
    }

    public static void FireCD(float index)
    {

        EntityManager.Instance.player.buffManager.AddBuff(new Buff_FireCD(index), 9999999);
    }
    public static void FireTimes(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_FireTimes((int)index), 9999999);
    }
    public static void MaxHealth(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_MaxHealth(index), 9999999);
    }
    public static void MoveSpeed(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_MoveSpeed(index), 9999999);
    }
    public static void Pentration(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_Pentration((int)index), 9999999);
    }
    public static void Range(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_Range(index), 9999999);
    }
    public static void ShotSpeed(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_ShotSpeed(index), 9999999);
    }
    public static void StopPower(float index)
    {
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_StopPower(index), 9999999);
    }
}
