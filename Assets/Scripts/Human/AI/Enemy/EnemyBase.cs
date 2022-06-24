using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NodeCanvas.BehaviourTrees;

public class EnemyBase : AiBase
{
    private float lastAnxiety = 0f;
    public BehaviourTree[] behaviourTrees = new BehaviourTree[4]; //四种焦虑情况下的行为树
    public BTBlockEnum BTBLock; //用于回血\换弹等情况下锁住当前行为树

    public LineRenderer testLine; //用于测试的线

    public HumanBase MostThreaten
    {
        get
        {
            var mostThreaten = Threaten.ElementAt(0);

            for (int i = 1; i < Threaten.Count; i++)
            {
                var threaten = Threaten.ElementAt(i);

                if (threaten.Value.Threaten > mostThreaten.Value.Threaten)
                {
                    mostThreaten = threaten;
                }
            }

            return mostThreaten.Key;
        }
    }

    public override void Init(string name="")
    {
        base.Init(name);
        //加载无状态下的行为树
        GetComponent<BehaviourTreeOwner>().StartBehaviour(behaviourTrees[(int)AnxietyEnum.NotAnxiety]);
    }

    private readonly string[] enemyMask = { "Player", "TeamMate" };

    public override void Update()
    {
        base.Update();
        UpdateBehaviourTree();
        DetectEnemy(enemyMask);

       // Debug.Log($"位置: {CurGrid}");
    }

    private void UpdateBehaviourTree()
    {
        if (BTBLock != BTBlockEnum.OnBlock && CalcAnxietyLevel(Anxiety) != CalcAnxietyLevel(lastAnxiety)) //允许换树&&跨越阈值
        {
            if (BTBLock == BTBlockEnum.OnChange)
            {
                BTBLock = BTBlockEnum.NoBlock;
            }

            GetComponent<BehaviourTreeOwner>().StartBehaviour(behaviourTrees[(int)CalcAnxietyLevel(Anxiety)]);
            LineManager.Instance.RemoveLine(testLine);
            accidentStop = true; //立即停止当前的动作
            stateManager.ChangeState<StandState>(); //自动切换回站立状态
        }

        if (BTBLock == BTBlockEnum.NoBlock)
        {
            lastAnxiety = Anxiety;
        }
    }

    private AnxietyEnum CalcAnxietyLevel(float anxiety)
    {
        if (anxiety > boundary2)
            return AnxietyEnum.SeriousAnxiety;
        else if (anxiety > boundary1)
            return AnxietyEnum.ModerateAnxiety;
        else if (anxiety > 0)
            return AnxietyEnum.MildAnxiety;
        else
            return AnxietyEnum.NotAnxiety;
    }

    public override void Die()
    {
        LineManager.Instance.RemoveLine(testLine);
        base.Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject humanGameObj = collision.gameObject;
        if (humanGameObj.GetComponent<HumanBase>() != null) //撞到的是人类
        {
            accidentStop = true;
            if (humanGameObj.GetComponent<EnemyBase>() == null) //撞到的不是敌人
            {
                TryAddThreaten(humanGameObj.GetComponent<HumanBase>(), 10, 5);
            }
        }
    }
}