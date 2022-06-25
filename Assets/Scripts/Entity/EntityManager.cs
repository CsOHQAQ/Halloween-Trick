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
    private float count;

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
        count = 8;
        allEntities = new List<Entity>();
        player = new PlayerEntity();
        player = ResourceManager.Instance.Instantiate("Prefabs/TestPlayer").GetComponent<PlayerEntity>();
    }

    private void Update()
    {
        count += Time.deltaTime;
        if (count >= 10)
        {
            Debug.Log("生成人物");
            count =0;
            SpawnEnemy(OutScreenPosition(),5);
        }
    }


    public void SpawnEnemy(Vector2 spawnPos,int spawnNum)
    {
        for(int i = 0; i < spawnNum; i++)
        {
            float x = RandNormalDistribution(spawnPos.x, 2), y = RandNormalDistribution(spawnPos.y, 2);
            Entity ent= ResourceManager.Instance.Instantiate("Prefabs/Children/BaseChild").GetComponent<Entity>();
            ent.Init();
            ent.transform.position = new Vector3(x, y);
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
                    x = Random.value * (right - left);
                    y = RandNormalDistribution(up + 5, 1);
                    break;
                }
            case 2:
                {
                    x = Random.value * (right - left);
                    y = RandNormalDistribution(down - 5, 1);
                    break;
                }
            case 3:
                {
                    x = RandNormalDistribution(left - 5, 1);
                    y = Random.value * (down - up);
                    break;
                }
            case 4:
                {
                    x = RandNormalDistribution(right+5, 1);
                    y = Random.value * (down - up);
                    break;
                }
            default:
                {
                    x = RandNormalDistribution(right + 5, 1);
                    y = Random.value * (down - up);
                    break;
                }
        }
        result = new Vector2(x, y);
        return result;

    }
}
