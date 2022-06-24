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
        GunSpark = shootPlace.GetComponent<ParticleSystem>();
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
        if (data.CurAmmo > 0 && CanShoot)
        {
            FireCDCount = 6f / data.ShotSpeed;
            data.CurAmmo--;
            CanShoot = false;
            /*
            bullet=ResourceManager.Instance.Instantiate("Prefabs/Bullet/Bullet").GetComponent<Bullet>();
            bullet.transform.position = shootPlace.position;
            bullet.Init(data.Pentration,data.StoppingPower,shootPlace.transform.position,Target);
            bullet.time.rigidbody2D.velocity=((Target - (Vector2)shootPlace.position).normalized*1000);
            Destroy(bullet.gameObject, Vector2.Distance(shootPlace.position, Target) / bullet.time.rigidbody2D.velocity.magnitude);
            */
            FireDetect(Target);
            GunSpark.Play();
            return true;
        }
        else
        {
            return false;

        }
    }

    public bool CanHit(Vector2 Target)
    {
        RaycastHit2D[] hit2D = Physics2D.RaycastAll(shootPlace.position, Target - (Vector2)shootPlace.position, Vector2.Distance(shootPlace.position, Target));

        for (int i = 0; i < hit2D.Length; i++)
        {
            if (LayerMask.LayerToName(hit2D[i].collider.gameObject.layer) == "Obstacle")
            {
                return false;
            }
        }
        return true;
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
