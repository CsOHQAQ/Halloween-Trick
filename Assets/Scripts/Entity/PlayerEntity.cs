using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerEntity : Entity
{
    public float HealBatteryRange=5;
    public float HealBatterySpeed=15;

    public override void Init()
    {
        base.Init();
        MaxHealth = tab.GetFloat("Character", "Player", "Health");
        CurHealth = MaxHealth;
        MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

        //测试用
        weaponManager.Add("ShotGun");

    }
    private void Start()
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * MoveSpeed * Time.deltaTime;
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

        // this.GetComponent<WeaponManager>().CurWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // 自动开火 往鼠标方向
        for (int i = 0; i < weaponManager.EquipWeapon.Count; i++)
        {
            weaponManager.EquipWeapon[i].Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }


        //测试部分
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("测试buff");
            TestBuff();
        }
        TestBattery();
    }

    private void TestBuff()
    {
        //测试部分
        Buff_Pentration testBuff = new Buff_Pentration(3);
        buffManager.AddBuff(testBuff, 5);
    }
    private void SpeedSlowDown()
    {

    }

	void TestBattery()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject go= ResourceManager.Instance.Instantiate("Prefabs/Battery/DamageBattery");
            go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
            go.GetComponent<DamageBattery>().Init();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Battery/NormalBattery");
            go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
            go.GetComponent<NormalBattery>().Init();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject go = ResourceManager.Instance.Instantiate("Prefabs/Battery/SlowBattery");
            go.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
            go.GetComponent<SlowBattery>().Init();
        }
    }
}