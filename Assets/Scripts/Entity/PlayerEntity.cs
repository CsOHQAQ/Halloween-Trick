using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerEntity : Entity
{
    public float HealBatteryRange=5;
    public float HealBatterySpeed=15;

    private bool IsThrow = true;
    private Animator animator;

    public override void Init()
    {
        base.Init();
        type = EntityType.Player;
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "Player", "Health");
        data.MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

        //测试用
        //weaponManager.Add("MachineGun");
        weaponManager.Add("HandGun");

    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        animator.SetBool("IsThrow",IsThrow);
        animator.SetBool("IsStay", true);
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x < 0) this.GetComponent<SpriteRenderer>().flipX = true;
        else this.GetComponent<SpriteRenderer>().flipX = false;

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * Data.MoveSpeed * Time.deltaTime;
            animator.SetBool("IsStay", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector2.right * Data.MoveSpeed * Time.deltaTime;
            animator.SetBool("IsStay", false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (Vector3)Vector2.up * Data.MoveSpeed * Time.deltaTime;
            animator.SetBool("IsStay", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3)Vector2.down * Data.MoveSpeed * Time.deltaTime;
            animator.SetBool("IsStay", false);
        }

        //测试按E关闭开火
        if (Input.GetKeyDown(KeyCode.E))
        {
            IsThrow = !IsThrow;
        }

        // this.GetComponent<WeaponManager>().CurWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // 自动开火 往鼠标方向
        if(IsThrow)
        for (int i = 0; i < weaponManager.EquipWeapon.Count; i++)
        {
            weaponManager.EquipWeapon[i].Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }


        //测试部分
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (flag == false)
            {
                Debug.Log("测试buff");
                TestBuff();
            }
            else
            {
                Debug.Log("取消测试buff");
                buffManager.RemoveAllBuff();
            }
            flag = !flag;
        }
        TestBattery();

        TestProcedure();
        // 获得全武器
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponManager.Add("ShotGun");
            weaponManager.Add("SubMachineGun");
            weaponManager.Add("MachineGun");
        }

        Debug.Log("MaxHealth" + Data.MaxHealth + "CurHealth" + Data.CurHealth);
    }

    private bool flag = false;

    private void TestBuff()
    {
        //测试部分
        //Buff_Pentration testBuff = new Buff_Pentration(100);
        //buffManager.AddBuff(testBuff, 100);
        //Buff_StopPower testBuff2 = new Buff_StopPower(-0.5f);
        //buffManager.AddBuff(testBuff2, 100);
        //Buff_ShotSpeed testBuff3 = new Buff_ShotSpeed(-0.9f);
        //buffManager.AddBuff(testBuff3, 100);
        Buff_MaxHealth testBuff4 = new Buff_MaxHealth(100f);
        buffManager.AddBuff(testBuff4, 100);
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
    void TestProcedure()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ProcedureManager.Instance.ChangeTo("ShopProcedure");
        }
    }
    public override void BeforeDestroy()
    {
        //Debug.Log("")
        ProcedureManager.Instance.ChangeTo("ShopProcedure");
    }
}