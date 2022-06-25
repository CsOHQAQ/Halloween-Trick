using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerEntity : Entity
{

    public override void Init()
    {
        base.Init();
        MaxHealth = tab.GetFloat("Character", "Player", "Health");
        MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

    }

    private void Awake()
    {
        base.Init();

        CurHealth = MaxHealth = tab.GetFloat("Character", "Player", "Health");
        MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

      
    }

    private void Start()
    {
        //测试用
        weaponManager.Add("ShotGun");
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * MoveSpeed*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector2.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (Vector3)Vector2.up * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3)Vector2.down * MoveSpeed * Time.deltaTime;
        }

        this.GetComponent<WeaponManager>().CurWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // 自动开火 往鼠标方向
        for (int i = 0; i < weaponManager.EquipWeapon.Count; i++)
        {
            weaponManager.EquipWeapon[i].Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newOne = ResourceManager.Instance.Instantiate("Prefabs/Battery/NormalBattery");
            newOne.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)+new Vector3(0,0,10);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject newOne = ResourceManager.Instance.Instantiate("Prefabs/Battery/DamageBattery");
            newOne.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }
    }
}
