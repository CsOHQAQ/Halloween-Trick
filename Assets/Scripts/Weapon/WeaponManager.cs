using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;  
/// <summary>
/// Manager用来给外部提供一个调用的接口，在内部的各种base中最终实现该功能
/// </summary>
public class WeaponManager : MonoBehaviour
{
    public List<WeaponBase> EquipWeapon;
    public WeaponBase CurWeapon;
    public Entity ent;
    public void Init()
    {
        EquipWeapon = new List<WeaponBase>();
    }
    //待做
    public void ChangeWeapon()
    {
        if (EquipWeapon.Count > 1)
        {

        }
    }

    /// <summary>
    /// 这个用于添加已经初始化过的武器，比如已经带着配件得武器
    /// </summary>
    /// <param name="weapon"></param>
    public void Add(WeaponBase weapon)
    {
        if (EquipWeapon.Count>=2)//丢弃武器
        {
            
        }

    }

    /// <summary>
    /// 这个用于添加尚未初始化的武器
    /// </summary>
    /// <param name="Name"></param>
    public void Add(string Name)
    {
        if (EquipWeapon.Count < 2)
        {
            GameObject g = ResourceManager.Instance.Instantiate("Prefabs/Weapon/Gun/"+Name,transform);
            CurWeapon = g.GetComponent<WeaponBase>();
            CurWeapon.manager = this;

            CurWeapon.Init();

            EquipWeapon.Add(CurWeapon);
        }
    }

    public void Reload()
    {
        CurWeapon.Reload();
    }
}
