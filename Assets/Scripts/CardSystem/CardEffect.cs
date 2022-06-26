using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;

/// <summary>
/// 此处纯存放对玩家的加强卡牌的使用效果
/// </summary>
public class CardEffect 
{
    public static void BaseSpread(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_BaseSpread(Index),9999999);
    }

    public static void FireCD(string index)
    {

        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_FireCD(Index), 9999999);
    }
    public static void FireTimes(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_FireTimes((int)Index), 9999999);
    }
    public static void MaxHealth(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_MaxHealth(Index), 9999999);
    }
    public static void MoveSpeed(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_MoveSpeed(Index), 9999999);
    }
    public static void Pentration(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_Pentration((int)Index), 9999999);
    }
    public static void Range(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_Range(Index), 9999999);
    }
    public static void ShotSpeed(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_ShotSpeed(Index), 9999999);
    }
    public static void StopPower(string index)
    {
        float Index = float.Parse(index);
        EntityManager.Instance.player.buffManager.AddBuff(new Buff_StopPower(Index), 9999999);
    }
    public static void UpgradeSubGun()
    {
        EntityManager.Instance.player.weaponManager.Add("SubMachineGun");
    }
    public static void UpgradeShotGun(string index)
    {
        EntityManager.Instance.player.weaponManager.Add("ShotGun");

    }
    public static void UpgradeMachineGun(string index)
    {
        EntityManager.Instance.player.weaponManager.Add("MachineGun");

    }
    public static void SummonDamageBattery(string index)
    {

        GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Battery/DamageBattery");
        go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        go.GetComponent<DamageBattery>().Init();
        EntityManager.Instance.allEntities.Add(go.GetComponent<Entity>());
    }
    public static void SummonSlowBattery(string index)
    {

        GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Battery/SlowBattery");
        go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        go.GetComponent<SlowBattery>().Init();
        EntityManager.Instance.allEntities.Add(go.GetComponent<Entity>());
    }
    public static void SummonNormalBattery(string index)
    {

        GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Battery/NormalBattery");
        go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        go.GetComponent<NormalBattery>().Init();
        EntityManager.Instance.allEntities.Add(go.GetComponent<Entity>());
    }

}
