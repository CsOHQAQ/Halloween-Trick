using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class WeaponBase : MonoBehaviour
{
    public WeaponData data;
    public bool CanShoot;
    private ParticleSystem GunSpark;
    private Transform shootPlace;

    private float FireCDCount;
    public virtual void Init()
    {
        shootPlace = transform.Find("ShootPlace");
        //GunSpark = shootPlace.GetComponent<ParticleSystem>();
        CanShoot = true;
        FireCDCount = 0;
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
            FireCDCount = data.FireCD;
            CanShoot = false;
            for(int i = 0; i < data.FireTimes; i++)
            {
                float RandAngle=Random.Range(-data.BaseSpread,data.BaseSpread);
                Bullet NewBullet = ResourceManager.Instance.Instantiate("Prefabs/Weapon/Bullet/Bullet").GetComponent<Bullet>();
                Vector2 StartPoint = (Vector2)(shootPlace.localPosition + this.transform.localPosition + this.transform.parent.position);
                NewBullet.Damage = data.StoppingPower;
                NewBullet.transform.position = StartPoint;
                NewBullet.speed = data.ShotSpeed;
                NewBullet.direction = Target -StartPoint;
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

    public void Reload()
    {
        data.CurAmmo = data.MaxAmmo;
    }
    public void Reload(int bulletNum)
    {
        data.CurAmmo += bulletNum;
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
}
