using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using QxFramework.Utilities;
using App.Common;
using UnityEngine.UI;

public class EntityManager : MonoSingleton<EntityManager>
{
    public PlayerEntity player;
    public List<Entity> allEntities;

    public float CurEnergy;
    private readonly float EnergyGap = 10;
    public float MaxEnergy = 0;
    public int EnergyLevel = 0;

    private float count;

    //protected TableAgent spawnTab;
    protected object[,] spawnData; //[行数, 数据类型(0-Type 1-Difficulty)

    // 开始游戏时间
    private GameDateTime StartTime;
    // 当前关卡持续时间分钟数（游戏内时间）
    public int MaxTime = 5 * GameDateTime.MinutesPerHour;
    // 目前已经经过的分钟数（游戏内时间）
    public int TimeDiff => (GameMgr.Get<IGameTimeManager>().GetNow() - StartTime).TotalMinutes;

    private List<Buff> ChildBuffTable = new List<Buff>();
    private readonly int InfiniteTime = int.MaxValue;

    private bool reminder1 = false,
                 reminder2 = false,
                 reminder3 = false;

    /// <summary>
    /// 返回一个给定范围?的服从正态分布的随机数
    /// </summary>
    /// <param name="avg">均值</param>
    /// <param name="sigma">标准差，所有取值范围可以视为落在3sigma的范围之内</param>
    /// <returns></returns>
    public float RandNormalDistribution(float avg, float sigma)
    {
        //Box-Muller算法
        float randValue, lengthValue;
        float u1 = Random.value, u2 = Random.value;

        randValue = Mathf.Cos(2 * Mathf.PI * u2) * Mathf.Sqrt(-2f * Mathf.Log(u1));
        //cos换成sin均服从正态分布的在（-1，1）的随机数

        randValue = randValue * sigma + avg;
        return randValue;
    }
    public void Init()
    {
        MaxEnergy = EnergyGap;
        CurEnergy = 0;
        allEntities = new List<Entity>();

        count = 4;
        allEntities = new List<Entity>();
        player = new PlayerEntity();
        player = ResourceManager.Instance.Instantiate("Prefabs/TestPlayer").GetComponent<PlayerEntity>();
        player.Init();
        player.LastInit();

        // 初始化小孩生成
        TableAgent spawnTab = new TableAgent();
        spawnTab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/ChildSpawn").text);
        System.Type[] types = { typeof(string), typeof(float) };
        spawnData = spawnTab.GetAllEntries("ChildSpawn", types);

        // 获取进入关卡时的时间
        StartTime = GameMgr.Get<IGameTimeManager>().GetNow();
    }

    private void Update()
    {


        if(ProcedureManager.Instance.Current is BattleProcedure)
        {
            count += Time.deltaTime;
            if (count >= 5)
            {
                Debug.Log("生成人物");
                count = 0;
                // 只在生成前维护Buff表 节省开销
                UpdateBuffTable();
                //SpawnEnemy(OutScreenPosition(), GetMaxDiffByTime());
                SpawnEnemy(GetMaxDiffByTime());

            }
            //if (GameMgr.Get<IGameTimeManager>().GetNow().Hours <= 1)
            //{
            //    ProcedureManager.Instance.ChangeTo("ShopProcedure");
            //}
            if (TimeDiff >= MaxTime)
            {
                ProcedureManager.Instance.ChangeTo("ShopProcedure");
            }
            else if (!reminder1 && TimeDiff >= MaxTime / 2)
            {
                reminder1 = true;
                UIManager.Instance.Open("ReminderUI", args: "时间过半，所有小朋友移速提升！");
            }
            else if (!reminder2 && TimeDiff >= MaxTime * 2 / 3)
            {
                reminder2 = true;
                UIManager.Instance.Open("ReminderUI", args: "时间过三分之二，所有远程小朋友精度和范围提升！");
            }
            else if (!reminder3 && TimeDiff >= MaxTime * 3 / 4)
            {
                reminder3 = true;
                UIManager.Instance.Open("ReminderUI", args: "时间过四分之三，所有远程小朋友射速和CD提升！");
            }
        }

        //Debug.Log(GetMaxDiffByTime());
    }

    private int GetMaxDiffByTime()
    {
        return Mathf.RoundToInt(4 * (1 + (float)TimeDiff / 360)); // 3个小时+初始的一倍
    }

    private void UpdateBuffTable()
    {
        ChildBuffTable.Clear();

        // 时间过半后，增强所有怪移速20%
        if (TimeDiff > MaxTime / 2)
        {
            ChildBuffTable.Add(new Buff_MoveSpeed(0.2f));
        }
        // 时间过2/3后，增强所有远程怪精度20%和范围20%
        if (TimeDiff > MaxTime * 2 / 3)
        {
            ChildBuffTable.Add(new Buff_BaseSpread(-0.2f));
            ChildBuffTable.Add(new Buff_Range(0.2f));
        }
        // 时间过3/4后，增强所有远程怪射速20%和射击CD20%
        if (TimeDiff > MaxTime * 3 / 4)
        {
            ChildBuffTable.Add(new Buff_ShotSpeed(0.2f));
            ChildBuffTable.Add(new Buff_FireCD(-0.2f));
        }

        // 增强生命值、DPS和武器伤害
        ChildBuffTable.Add(new Buff_MaxHealth((float)TimeDiff / 360)); // 3个小时+初始的一倍
        ChildBuffTable.Add(new Buff_DPS((float)TimeDiff / 360)); // 3个小时+初始的一倍
        ChildBuffTable.Add(new Buff_StopPower((float)TimeDiff / 360)); // 3个小时+初始的一倍
    }

    public void SpawnEnemy(int maxDiff)
    {
        float totalDiff = 0;
        int randRow;
        float x, y;
        string childType;
        float difficulty;
        Entity ent;
        Vector2 spawnPos;

        do
        {
            //randRow = Random.Range(0, spawnData.GetUpperBound(0) + 1);

            // 前12小时不会生成远程怪，前18小时不会生成孩子王
            int maxRow;
            if (TimeDiff > 18 * GameDateTime.MinutesPerHour)
            {
                maxRow = 76;
            }
            else if (TimeDiff > 12 * GameDateTime.MinutesPerHour)
            {
                maxRow = 75;
            }
            else
            {
                maxRow = 60;
            }
            randRow = Random.Range(0, maxRow);

            childType = (string)spawnData[randRow, 0];
            difficulty = (float)spawnData[randRow, 1];
            ent = ResourceManager.Instance.Instantiate("Prefabs/Children/" + childType).GetComponent<Entity>();
            ent.Init();
            ent.LastInit();
            spawnPos = OutScreenPosition();
            x = RandNormalDistribution(spawnPos.x, 2);
            y = RandNormalDistribution(spawnPos.y, 2);
            ent.transform.position = new Vector3(x, y);
            foreach (Buff buff in ChildBuffTable)
            {
                ent.buffManager.AddBuff(buff, InfiniteTime);
            }

            totalDiff += difficulty;
            allEntities.Add(ent);
        }
        while (totalDiff < maxDiff);

        //ent = ResourceManager.Instance.Instantiate("Prefabs/Children/ChildKing").GetComponent<Entity>();
        //ent.Init();
        //x = RandNormalDistribution(spawnPos.x, 2);
        //y = RandNormalDistribution(spawnPos.y, 2);
        //ent.transform.position = new Vector3(x, y);
    }

    public void SpawnEnemy(Vector2 spawnPos, int maxDiff)
    {
        float totalDiff = 0;
        int randRow;
        float x, y;
        string childType;
        float difficulty;
        Entity ent;

        do
        {
            //randRow = Random.Range(0, spawnData.GetUpperBound(0) + 1);

            // 前12小时不会生成远程怪，前18小时不会生成孩子王
            int maxRow;
            if (TimeDiff > 18 * GameDateTime.MinutesPerHour)
            {
                maxRow = 76;
            }
            else if (TimeDiff > 12 * GameDateTime.MinutesPerHour)
            {
                maxRow = 75;
            }
            else
            {
                maxRow = 60;
            }
            randRow = Random.Range(0, maxRow);

            childType = (string)spawnData[randRow, 0];
            difficulty = (float)spawnData[randRow, 1];
            ent = ResourceManager.Instance.Instantiate("Prefabs/Children/" + childType).GetComponent<Entity>();
            ent.Init();
            ent.LastInit();
            x = RandNormalDistribution(spawnPos.x, 2);
            y = RandNormalDistribution(spawnPos.y, 2);
            ent.transform.position = new Vector3(x, y);
            foreach (Buff buff in ChildBuffTable)
            {
                ent.buffManager.AddBuff(buff, InfiniteTime);
            }

            totalDiff += difficulty;
            allEntities.Add(ent);
        }
        while (totalDiff < maxDiff);

        //ent = ResourceManager.Instance.Instantiate("Prefabs/Children/ChildKing").GetComponent<Entity>();
        //ent.Init();
        //x = RandNormalDistribution(spawnPos.x, 2);
        //y = RandNormalDistribution(spawnPos.y, 2);
        //ent.transform.position = new Vector3(x, y);
    }

    public void AddEnergy(Entity ent)
    {
        TableAgent tab = new TableAgent();
        tab.Add(ResourceManager.Instance.Load<TextAsset>("Text/Table/Character").text);
        
        CurEnergy += tab.GetFloat("Character", ent.type.ToString(), "Energy");
CardManager.Instance.CurMoney += tab.GetFloat("Character", ent.type.ToString(), "Energy");
        Debug.Log("增加能量" + tab.GetFloat("Character", ent.type.ToString(), "Energy"));
        if (CurEnergy >= MaxEnergy)
        {
            CurEnergy = MaxEnergy;
            if (CardManager.Instance.HandCard.Count < 3)
            {
                CardManager.Instance.DrawCardFromCargo();
                CurEnergy = 0;
                EnergyLevel++;
                MaxEnergy += EnergyGap * (1 + (float)EnergyLevel / 10);
            }
        }
    }

    public Vector2 OutScreenPosition()
    {
        float left, right, up, down;
        left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        down= Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        right=Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)).x;
        up= Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)).y;
        int direction = Random.Range(1, 5);
        Vector2 result = new Vector2();
        float x, y;
        switch (direction)
        {
            case 1://上
                {
                    x = Random.value * (right - left) + left;
                    y = RandNormalDistribution(up + 5, 1);
                    break;
                }
            case 2:
                {
                    x = Random.value * (right - left) + left;
                    y = RandNormalDistribution(down - 5, 1);
                    break;
                }
            case 3:
                {
                    x = RandNormalDistribution(left - 5, 1);
                    y = Random.value * (down - up) + down;
                    break;
                }
            case 4:
                {
                    x = RandNormalDistribution(right + 5, 1);
                    y = Random.value * (down - up) + down;
                    break;
                }
            default:
                {
                    x = RandNormalDistribution(right + 5, 1);
                    y = Random.value * (down - up) + down;
                    break;
                }
        }
        result = new Vector2(x, y);
        return result;

    }
    public void ClearAll()
    {
        UIManager.Instance.CloseAll();
        foreach(var ent in allEntities)
        {
            if (ent != null)
            {
                Destroy(ent.gameObject);
            }
        }
        allEntities.Clear();
        Destroy(player.gameObject);
    }
}