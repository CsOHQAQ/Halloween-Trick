using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class HumanBase : MonoBehaviour
{
    public HumanData data;
    public Timeline time;
    public RigidbodyTimeline2D body;
    public BuffManager buffManager;
    public WeaponManager weaponManager;
    public StateManager stateManager;
    //超速的刹车速度
    public float OverSpeedCorrect=40;
    //对浮点运算的误差舍弃范围
    public float acceptedError;
    [HideInInspector]
    public bool isMoveX;
    [HideInInspector]
    public bool isMoveY;
    public Vector2 Target=new Vector2();
    public bool isRotate = false;
    public bool isLookAtMoveDirection = true;


    //[SerializeField] 
    //private ParticleSystem bloodPrefab;

    public string CurGrid //在小地图内格子的名字
        => $"({(int)(transform.position.x + 5) / 10 + 1},{(int)(transform.position.y + 5) / 10 + 1})";
    /// <summary>
    /// 将来会换成按照姓名刷新人物,目前现将就用着罢
    /// </summary>
    public virtual void Init(string name="NormalHuman")
    {
        if (name == ""||name== null)//防止名字为空
        {
            name = "NormalHuman";
        }
        data = new HumanData(name);
        data.ResetState();
        buffManager = GetComponent<BuffManager>();
        buffManager.Init();
        time = GetComponent<Timeline>();
        time.globalClockKey = "InGame";
        body = time.rigidbody2D;
        //.Log("获取" + transform.name + "的RigidBody" + (body is null));


        weaponManager = transform.Find("Weapon").GetComponent<WeaponManager>();
        weaponManager.Init();
        stateManager = GetComponent<StateManager>();
        StateManager.CloneStates(stateManager);
        stateManager.Init();

        //UI打开
        Transform StateUI = transform.Find("UIOpener");
        StateUI.GetComponent<UIOpener>().OpenStateUI();

        //以下为测试用
        Debug.Log(transform.name + "初始化中");
        weaponManager.Add("HG_HS2000");
        weaponManager.CurWeapon.Reload();
        data.backpack.itemPiles.Add(new ItemPile {item=GameManager.Get<IItemManager>().GetItemStatus(1001),CurrentPile= 40,CurrentPosID=0});
    }
    

    // Update is called once per frame
    public  virtual void Update()
    {
        //朝向目标
        if (!isLookAtMoveDirection)
            LookingAt(Target);
        else
        {
            if (body.velocity.magnitude > 0.05f)
            {
                LookingAt((Vector2)this.transform.position+body.velocity.normalized);
            }
            
        }
            
        //控制速度上限
        if (!isMoveX || Mathf.Abs(body.velocity.x) > data.MoveSpeed) SlowDownX();
        if (!isMoveY || Mathf.Abs(body.velocity.y) > data.MoveSpeed) SlowDownY();
        isMoveX = false;
        isMoveY = false;

        //控制饥饿的buff
        float hungerPercent = data.CurrentHunger / data.MaxHunger;
        if (hungerPercent < 0.5f&&hungerPercent>0.3f)
        {
            //buffManager.AddBuff()
        }
    }
    

    public virtual void FixedUpdate()
    {
        float fixdeltatime = Timekeeper.instance.Clock("SurviveTime").fixedDeltaTime;
        data.CurrentHunger -= data.ConsumeSpeed / 1440 * fixdeltatime;
        data.HpChange(-data.VHealthDropSpd / 1440 * fixdeltatime, HpChangeState.VirtualHp);
    }

    /// <summary>
    /// 添加一次Acceleration的速度
    /// </summary>
    /// <param name="Towards"></param>
    public void Move(Vector2 Towards)
    {
        body.velocity += Towards.normalized * data.Acceleration*time.deltaTime;
        body.velocity = body.velocity.normalized * Mathf.Min(data.MoveSpeed, body.velocity.magnitude);
        if (Towards.x != 0)
            isMoveX = true;
        if (Towards.y != 0)
            isMoveY = true;
    }

    public void SlowDownX()
    {
        body.velocity -= new Vector2(body.velocity.normalized.x>0?1:-1 * Mathf.Min(Mathf.Abs( body.velocity.x), OverSpeedCorrect * time.deltaTime),0);
    }
    public void SlowDownY()
    {
        body.velocity -= new Vector2(0,body.velocity.normalized.y>0?1:-1 * Mathf.Min(Mathf.Abs( body.velocity.y), OverSpeedCorrect * time.deltaTime));
    }
    public void Run()
    {
        buffManager.AddBuff(new Buff_Run(), time.deltaTime);
    }
    /// <summary>
    /// 看向目标
    /// </summary>
    /// <param name="t">t为目标的世界坐标 </param>
    public virtual void LookingAt(Vector2 t)
    {
        if (t != null)
        {
            isRotate = true;

            float angle, targetAngle;
            angle = targetAngle = Vector2.SignedAngle(Vector2.up, t - (Vector2)transform.position);
            while (angle < transform.rotation.eulerAngles.z)
            {
                angle += 360;
            }

            if (angle - transform.rotation.eulerAngles.z < 180)
            {
                angle = transform.rotation.eulerAngles.z + Mathf.Min(data.AimSpeed * time.deltaTime * 80, angle - transform.rotation.eulerAngles.z);
            }
            else
            {
                angle = transform.rotation.eulerAngles.z - Mathf.Min(data.AimSpeed * time.deltaTime * 80, 360 - (angle - transform.rotation.eulerAngles.z));
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);
            if (Mathf.Abs(targetAngle - ToSignedAngle(transform.rotation.eulerAngles.z)) < acceptedError)
            {
                isRotate = false;
            }

            float ToSignedAngle(float a)
            {
                if (a > 180)
                {
                    while (a > 180)
                    {
                        a -= 360;
                    }
                }
                else if (a <= -180)
                {
                    while (a <= -180)
                    {
                        a += 360;
                    }
                }

                return a;
            }
        }
    }
    public virtual bool Fire(float SpreadRadius)
    {
        Vector2 Spread = new Vector2(Random.Range(-SpreadRadius,SpreadRadius),Random.Range(-SpreadRadius,SpreadRadius));
        return weaponManager.CurWeapon.Fire(Target+Spread);
    }
    public virtual void BeingFired(ref BulletData bulletData)
    {
        ParticleSystem p = this.GetComponentInChildren<ParticleSystem>();


        //if(bloodPrefab != null)
        //{
        //    bloodPrefab.transform.localPosition = new Vector3(0, 0, 0);
        //    bloodPrefab.transform.LookAt(bulletData.From, Vector3.up);
        //    bloodPrefab.Play();
        //}
        p.Play();

        body.velocity += (bulletData.Target - bulletData.From).normalized * bulletData.StoppingPower;
        data.HpChange(-bulletData.StoppingPower, HpChangeState.AllHp);
        bulletData.Pentration -= bulletData.StoppingPower;

        if (data.CurrentHealth <= 0)
            Die();
    }
    /// <summary>
    /// 这个函数仅作为数值更改以及事件系统预留，是一个底层的函数
    /// </summary>
    /// <param name="changehp"></param>
    public void FirstAid(float changehp)
    {
        data.HpChange(changehp, HpChangeState.VirtualHp); //回复的是虚血
    }
    /// <summary>
    /// 底层换弹函数
    /// </summary>
    public void Reload()
    {
        bool exBullet = weaponManager.CurWeapon.data.CurAmmo > 0&&(weaponManager.CurWeapon.data.CurAmmo<= weaponManager.CurWeapon.data.MaxAmmo);
        int needBullet = weaponManager.CurWeapon.data.MaxAmmo - weaponManager.CurWeapon.data.CurAmmo;
        if (GameManager.Get<IItemManager>().CheckItemEnough(1001, needBullet, new CargoData[] { data.backpack }))
        {
            Debug.Log("已完成换弹");
            GameManager.Get<IItemManager>().RemoveItemByID(1001, needBullet, new CargoData[] { data.backpack });
            weaponManager.CurWeapon.Reload();

            if (exBullet && GameManager.Get<IItemManager>().GetItemCount(1001, new CargoData[] { data.backpack }) > 0)//EX上弹
            {
                weaponManager.CurWeapon.Reload(1);
                GameManager.Get<IItemManager>().RemoveItemByID(1001, 1, new CargoData[] { data.backpack });
            }
        }
        else
        {
            Debug.Log("#Weapon子弹不足，仅添加"+ GameManager.Get<IItemManager>().GetItemCount(1001, new CargoData[] { data.backpack })+"颗子弹");
            weaponManager.CurWeapon.Reload(GameManager.Get<IItemManager>().GetItemCount(1001, new CargoData[] { data.backpack }));
            GameManager.Get<IItemManager>().RemoveItemByID(1001,GameManager.Get<IItemManager>().GetItemCount(1001, new CargoData[] { data.backpack }), new CargoData[] { data.backpack });
        }
    }

    public virtual void Die()
    {
        OnDestroy();
        return;
    }

    private void OnDestroy()
    {
        Transform StateUI = transform.Find("UIOpener");
        //StateUI.GetComponent<UIOpener>().DestroyStateUI();
        StateUI.GetComponent<UIOpener>().DestroyAllUI();
        Destroy(gameObject);
    }
}
