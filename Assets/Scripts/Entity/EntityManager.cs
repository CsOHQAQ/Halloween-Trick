using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using QxFramework.Utilities;
using App.Common;
using UnityEngine.UI;

public class EntityManager : MonoSingleton<EntityManager>
{
    //随机生成人物的计时器
    float counter;
    //生成下一个人物所需时间
    float spawnTime=5;
    //每日收工时间
    int endtime=21;
    public  List<Entity> entityList;
    public Dictionary<int, Entity> entityDic;

    public float baseValue=5;//配对的基础价值
    public float matchSuccessValue = 5;//tag匹配的增加价值
	public const int minCount=3;
    public const int maxCount=8;

    public const float LifeTime = 90;

    private Dictionary<string, GameDateTime> specialDic;



    private void Awake()
    {
        entityList = new List<Entity>();
        entityDic = new Dictionary<int, Entity>();

        specialDic = new Dictionary<string, GameDateTime>();
    }
    private void Update()
    {
    }

}
