using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class WeaponBase : MonoBehaviour
{
    public WeaponData data;//这个是未计算buff的，不要直接调用
    public WeaponDataBuffed Data;
    public bool CanShoot;
    private Transform shootPlace;
    private float minIntervalAngle = 1;
    private float FireCDCount;
    public WeaponManager manager;
    public virtual void Init()
    {
        shootPlace = transform.Find("ShootPlace");
        CanShoot = true;
        FireCDCount = 0;
        Data = new WeaponDataBuffed();
        Data.changer = manager.ent.buffManager.weaponChanger;
        Data.data = data;

    }
    public void FixedUpdate()
    {
        if (FireCDCount > 0)
        {
            FireCDCount -= Time.fixedDeltaTime;
            if (FireCDCount <= 0)
            {
                FireCDCount = 0;
                CanShoot = true;

            }
        }
    }

    /// <summary>
    /// 执行单次开火操作
    /// </summary>
    public bool Fire(Vector2 Target)
    {
        if (CanShoot)
        {
            FireCDCount = Data.FireCD;
            CanShoot = false;
            for(int i = 0; i < Data.FireTimes; i++)
            {
                float dAngleRange = 2 * Data.BaseSpread / Data.FireTimes;
                float RandAngle=Random.Range(dAngleRange*i-Data.BaseSpread,dAngleRange*(i+1) - Data.BaseSpread) +(i-(int)Data.FireTimes/2)*minIntervalAngle;
                //Debug.Log(RandAngle);
                Bullet NewBullet = ResourceManager.Instance.Instantiate("Prefabs/Weapon/Bullet/Bullet").GetComponent<Bullet>();
                Vector2 StartPoint = (Vector2)(shootPlace.localPosition + this.transform.localPosition + this.transform.parent.position);
                NewBullet.Damage = Data.StoppingPower;
                NewBullet.transform.position = StartPoint;
                NewBullet.Start = StartPoint;
                NewBullet.Range = Data.Range;
                NewBullet.speed = Data.ShotSpeed;
                NewBullet.direction = Target -StartPoint;
                NewBullet.Pentration = Data.Pentration;
                NewBullet.layer = manager.ent.gameObject.layer;

                NewBullet.transform.RotateAround(StartPoint, Vector3.forward, RandAngle);
            }
            
            //GunSpark.Play();
            return true;
        }
        else
        {
            return false;

        }
    }

    void FireDetect(Vector2 Target)
    {
        //BulletData bullet = new BulletData(data, shootPlace.position, Target, 
           // transform.parent.parent.gameObject.GetComponent<HumanBase>());

        //Hit2D：正经的枪打出的射线检测
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(shootPlace.position, Target - (Vector2)shootPlace.position, Vector2.Distance(shootPlace.position, Target));
        //DrawHit2D:为了绘制枪线而反向打的一条射线
        RaycastHit2D[] DrawHit2D = Physics2D.RaycastAll(Target, (Vector2)shootPlace.position - Target, Vector2.Distance(shootPlace.position, Target));

        Debug.DrawLine(shootPlace.position, Target, Color.red);
        
        List<Vector2[]> DrawPosition = new List<Vector2[]>();
        if (hit2D.Length == 0)
        {
            DrawPosition.Add(new Vector2[2] { shootPlace.position, Target });
        }
        else
        {
            DrawPosition.Add(new Vector2[2] { shootPlace.position, new Vector2() });
        }

        Vector2 drawEndPosition = new Vector2();

        //冗长的射线检测实现
        for (int i = 0; i < hit2D.Length; i++)
        {
            drawEndPosition = hit2D[i].point;
        }
    }
    public class WeaponDataBuffed
    {
        public WeaponData data;
        public WeaponDataChanger changer;
        public int Pentration
        {
            get
            {
                return data.Pentration + changer.pentrationPlu;
            }
        }
        public float StoppingPower
        {
            get
            {
                return data.StoppingPower * changer.stopPowerPcnt;
            }
        }
        public float ShotSpeed
        {
            get
            {
                return data.ShotSpeed * changer.shotSpeedPcnt; 
            }
        }
        public float BaseSpread
        {
            get
            {
                return data.BaseSpread * changer.baseSpreadMul;
            }
        }
        public float FireTimes
        {
            get
            {
                return data.FireTimes + changer.fireTimesPlu;
            }
        }
        public float FireCD
        {
            get
            {
                return data.FireCD * changer.fireCDMul;
            }
        }
        public float Range
        {
            get
            {
                return data.Range * changer.rangePlu;
            }
        }
    }
}

