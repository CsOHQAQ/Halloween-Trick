using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;
using QxFramework.Core;
public class WeaponBase : MonoBehaviour
{
    public WeaponData data;
    public bool CanShoot;
    private ParticleSystem GunSpark;
    private Transform shootPlace;
    private Timeline time;

    private float FireCDCount;
    public virtual void Init()
    {
        shootPlace = transform.Find("ShootPlace");
        GunSpark = shootPlace.GetComponent<ParticleSystem>();
        time = GetComponent<Timeline>();
        time.globalClockKey = "InGame";
        CanShoot = true;
        FireCDCount = 0;
    }
    public void FixedUpdate()
    {
        if (FireCDCount > 0)
        {
            FireCDCount -= time.fixedDeltaTime;
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
        BulletData bullet = new BulletData(data, shootPlace.position, Target, 
            transform.parent.parent.gameObject.GetComponent<HumanBase>());

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
            switch (LayerMask.LayerToName(hit2D[i].collider.gameObject.layer))
            {
                case "Obstacle":
                    {
                        Vector2 Px = new Vector2(1, 0);
                        float angle = Vector2.Angle(Px, Target - (Vector2)shootPlace.position);
                        double distance1 = Vector2.Distance(shootPlace.position, Target);
                        double length1 = Vector2.Distance(shootPlace.position, hit2D[i].transform.position);
                        ObstacleBase obs = hit2D[i].transform.GetComponent<ObstacleBase>();
                        if (!obs.Intercept(ref bullet, distance1, length1, angle))
                        {
                            Debug.Log("从上面飘过去了");
                            Debug.DrawLine(hit2D[i].point, DrawHit2D[DrawHit2D.Length - 1 - i].point, Color.red);   
                        }
                        else
                        {
                            DrawPosition[DrawPosition.Count - 1][1] = drawEndPosition;
                            DrawPosition.Add(new Vector2[2] {DrawHit2D[DrawHit2D.Length-i-1].point,new Vector2() });
                        }
                        break;
                    }
                case "Detect":
                    {
                        if (bullet.Attacker.gameObject.layer == LayerMask.NameToLayer("Enemy")
                            && hit2D[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) //敌人不将同类视为威胁
                        {
                            break;
                        }
                        
                        AiBase target = hit2D[i].transform.gameObject.GetComponent<AiBase>(); //玩家不可能有这个东西(Bullet)
                        target.TryAddThreaten(bullet.Attacker, bullet.StoppingPower * (2 - target.CurHp / target.MaxHp), 3);
                        break;
                    }
                case "Player":
                    {
                        hit2D[i].transform.GetComponent<HumanBase>().BeingFired(ref bullet);
                        break;
                    }
                case "Enemy":
                    {
                        hit2D[i].transform.GetComponent<HumanBase>().BeingFired(ref bullet);
                        break;
                    }
                case "TeamMate":
                    {
                        hit2D[i].transform.GetComponent<HumanBase>().BeingFired(ref bullet);
                        break;
                    }
                case "GridCheck":
                    {

                        break;
                    }
                default:
                    {
                        Debug.LogWarning("检测到的物体" + hit2D[i].transform.name + "并未定义！");
                        break;
                    }
            }

            //关于地图块威胁程度更改的内容
            if (hit2D[i].collider.GetComponent<GridInfo>() != null)
            {
                hit2D[i].collider.GetComponent<GridInfo>().threatLeve += bullet.StoppingPower / 7; //测试 伤害/7
            }
            //IK 

            if (bullet.StoppingPower <= 0 || bullet.Pentration <= 0)//子弹已走完
            {
                DrawPosition.RemoveAt(DrawPosition.Count - 1);
                break;
            }
            if (i == hit2D.Length - 1)
            {
                DrawPosition[DrawPosition.Count - 1][1] = drawEndPosition;
            }
        }

        foreach(Vector2[] pos in DrawPosition)
        {
            Debug.DrawLine(pos[0], pos[1]);
            LineManager.Instance.SpawnLine(pos[0],pos[1],0.5f,"BulletTrail");
        }
    }
}
