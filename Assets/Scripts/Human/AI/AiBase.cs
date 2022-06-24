using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using System.Linq;

public class AiBase : HumanBase
{
    public CommandManager cmdManager;

    //移动相关
    //private LineRenderer moveLine;
    public bool isMove = false;
    public bool accidentStop = false; //因为特殊原因停止 eg撞墙 被攻击到(enemy)等
    private Vector2 movePos;

    //焦虑相关
    public float Anxiety;
    public Dictionary<HumanBase, ThreatenBase> Threaten;
    public float arg1; //焦虑数值系数
    public float arg2; //警戒时间系数
    public float arg3; //焦虑降低系数
    public float boundary1; //焦虑阈值1
    public float boundary2; //焦虑阈值2

    //视野相关
    public float eyeViewDistance; //视野距离
    public float viewAngle; //视野角度

    public float MaxHp => data.MaxHealth; //行为树中无法直接读取非monobehavior子类下的属性 额外创建一个属性
    public float CurHp => data.CurrentHealth;
    public float MaxAmmo => weaponManager.CurWeapon.data.MaxAmmo;
    public float CurAmmo => weaponManager.CurWeapon.data.CurAmmo;

    public override void Init(string name)
    {
        base.Init(name);
        cmdManager = GetComponent<CommandManager>();
        cmdManager.Init();
        Threaten = new Dictionary<HumanBase, ThreatenBase>();
    }

    //public void MoveToGrid(string pos)
    //{
    //    (int x, int y) = pos.GetXYByString();
    //    MoveTo(new Vector2((x - 1) * 10, (y - 1) * 10));

    //    GameObject.Find($"subMap/{pos}").GetComponent<GridInfo>().booked = this;
    //    if (x - CurGrid.GetXYByString().x != 0 && y - CurGrid.GetXYByString().y != 0) //斜线走的话 要设置三格
    //    {
    //        GameObject.Find($"subMap/({CurGrid.GetXYByString().x},{y})").GetComponent<GridInfo>().booked = this;
    //        GameObject.Find($"subMap/({x},{CurGrid.GetXYByString().y})").GetComponent<GridInfo>().booked = this;
    //    }
    //}

    public void MoveToGrid(int gridX, int gridY)
    {
        gridX--;
        gridY--;
        MoveTo(new Vector2(gridX * 10, gridY * 10));

        GameWatcher.Instance.CurSituationManager.GridsInSubMap[gridX, gridY].booked = this;
        if (gridX - CurGrid.GetXYByString().x + 1 != 0 
            && gridY - CurGrid.GetXYByString().y + 1 != 0) //斜线走的话 要设置三格
        {
            GameWatcher.Instance.CurSituationManager.GridsInSubMap[CurGrid.GetXYByString().x - 1, gridY].booked = this;
            GameWatcher.Instance.CurSituationManager.GridsInSubMap[gridX, CurGrid.GetXYByString().y - 1].booked = this;
        }
    }

    public void MoveTo(Vector2 pos)
    {
        //if (isMove) //不需要线后可以删除
        //{
        //    LineManager.Instance.RemoveLine(moveLine);
        //}

        movePos = pos;
        isMove = true;
        stateManager.ChangeState<MoveState>();
        //moveLine = LineManager.Instance.SpawnLine((Vector2)transform.position, pos, 0.5f, "Line_Yellow"); //生成一条到目标的线
    }

    public override void Update()
    {
        base.Update();

        DoMove();
        UpdateThreaten();
        UpdateAnxiety();
    }

    private void DoMove()
    {
        if (isMove)
        {
            if (Vector2.Distance(transform.position, movePos) < acceptedError * 2 || accidentStop /*IsCrashed()*/)
            {
                //accidentStop = false; 注释掉方便行为树节点读取情况 但需要手动修改此值
                isMove = false;
                stateManager.ChangeState<StandState>();
                //LineManager.Instance.RemoveLine(moveLine);
            }
            else
            {
                Move(movePos - (Vector2)transform.position);
            }
        }

        //bool IsCrashed()
        //{
        //    RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, 
        //        movePos - (Vector2)transform.position, Vector2.Distance(movePos, transform.position));

        //    if (hit2D.Length > 1 && hit2D[1].distance < acceptedError + GetComponent<CircleCollider2D>().radius)
        //    {
        //        return true;
        //    }

        //    return false;    
        //}
    }

    private void UpdateThreaten() //随时间移除记录的威胁
    {
        for (int i = 0; i < Threaten.Count; i++)
        {
            var threaten = Threaten.ElementAt(i);

            if (threaten.Key.data.CurrentHealth <= 0) //如果已经威胁已经死亡则移除
            {
                Threaten.Remove(threaten.Key);
                i--;
            }
            else if (threaten.Value.AlertTime > 0) //警戒时间内
            {
                Threaten[threaten.Key] =
                    new ThreatenBase(threaten.Value.Threaten, threaten.Value.AlertTime - time.deltaTime, this);
            }
            else //警戒时间结束，线性减少威胁
            {
                float newThr = threaten.Value.Threaten - time.deltaTime * 3 * arg3; //测试，1s减3
                if (newThr > 0)
                {
                    Threaten[threaten.Key] = new ThreatenBase(newThr, 0, this);
                }
                else
                {
                    Threaten.Remove(threaten.Key);
                    i--;
                }
            }
        }
    }

    private void UpdateAnxiety() //随时间刷新焦虑值
    {
        Anxiety = 0;

        if (Threaten.Count != 0)
        {
            foreach (var threaten in Threaten)
            {
                Anxiety += threaten.Value.Threaten;
            }
        }
    }

    public void TryAddThreaten(HumanBase humanBase, float threaten, float alertTime)
    {
        if (Threaten.ContainsKey(humanBase))
        {
            Threaten[humanBase] = new ThreatenBase(threaten + Threaten[humanBase].Threaten,
                Mathf.Max(alertTime, Threaten[humanBase].AlertTime), this);
        }
        else
        {
            Threaten.Add(humanBase, new ThreatenBase(threaten, alertTime, this));
        }
    }

    public void TryKeepThreaten(HumanBase humanBase, float minThreaten, float minAlertTime) //将一个目标加入威胁列表
    {
        if (Threaten.ContainsKey(humanBase))
        {
            Threaten[humanBase] = new ThreatenBase(Mathf.Max(Threaten[humanBase].Threaten, minThreaten), 
                Mathf.Max(Threaten[humanBase].AlertTime, minAlertTime), this);
        }
        else
        {
            Threaten.Add(humanBase, new ThreatenBase(minThreaten, minAlertTime, this));
        }
    }

    public void TryNormalFire() //如果打不中就不发射
    {
        if (weaponManager.CurWeapon.CanHit(Target))
        {
            Fire(data.Accuracy);
        }
    }

    public void DetectEnemy(string[] enemyMask) //查找视野范围内的敌对生物 敌对生物由enemyMask指定
    {
        Collider2D[] SpottedEnemies =
            Physics2D.OverlapCircleAll(transform.position, eyeViewDistance, LayerMask.GetMask(enemyMask)); //附近的敌对单位

        for (int i = 0; i < SpottedEnemies.Length; i++) //检测每一个敌对单位是否在视野区中
        {
            Vector2 pos = SpottedEnemies[i].transform.position; //敌对单位的位置
            if (Vector2.Angle(Target - (Vector2)transform.position, pos - (Vector2)transform.position) <= viewAngle / 2 //是否在视野内 生物一定看向Target 即Target就是对着的方向的向量
                && weaponManager.CurWeapon.CanHit(pos)) //有没有障碍物
            {
                //Debug.Log($"发现目标{SpottedEnemies[i].transform.position}");
                Debug.DrawRay(transform.position, pos - (Vector2)transform.position, Color.yellow);

                HumanBase humanBase = SpottedEnemies[i].GetComponent<HumanBase>();
                TryKeepThreaten(humanBase, 5, 3);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HumanBase>() != null) //撞到的是人类
        {
            accidentStop = true;
        }
    }
}
