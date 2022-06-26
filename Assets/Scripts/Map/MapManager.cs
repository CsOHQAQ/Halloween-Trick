using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
using QxFramework.Utilities;

public class MapManager : MonoSingleton<MapManager>
{
    // 需要补充地图的方位
    //public PosInNeed posInNeed = PosInNeed.NONE;
    // 四张地图数据
    public MapData[] maps = new MapData[4];
    private float rendWidth, rendHeight;

    public void Init()
    {
        // 创建四个地图物体
        for (int i = 0; i < 4; i++)
        {
            maps[i] = new MapData(ResourceManager.Instance.Instantiate("Prefabs/Map/map").GetComponent<SpriteRenderer>());
            maps[i].render.name = "map_" + i;
            maps[i].render.transform.parent = gameObject.transform;
        }
        // 设置第一个地图处于使用中
        maps[0].isFree = false;
        rendWidth = maps[0].render.bounds.size.x;
        rendHeight = maps[0].render.bounds.size.y;
        //初始化把其他几张地图赛旁边去
        maps[1].render.transform.position += new Vector3(rendWidth, 0, 0);
        maps[2].render.transform.position += new Vector3(-rendWidth, 0, 0);
        maps[3].render.transform.position += new Vector3(0,rendHeight, 0);
    }

    private void Update()
    {
        // 记录端点
        float camLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float camDown = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float camRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)).x;
        float camUp = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)).y;
        List<(float x, float y)> points = new List<(float x, float y)>();
        points.Add((camLeft, camDown));
        points.Add((camLeft, camUp));
        points.Add((camRight, camDown));
        points.Add((camRight, camUp));

        // 更新空闲状态+去除已经在的点
        for (int i = 0; i < 4; i++)
        {
            if (!maps[i].isFree)
            {
                SpriteRenderer render = maps[i].render;
                float mapLeft = render.transform.position.x - rendWidth / 2;
                float mapDown = render.transform.position.y - rendHeight / 2;
                float mapRight = render.transform.position.x + rendWidth / 2;
                float mapUp = render.transform.position.y + rendHeight / 2;

                int beforeCount = points.Count;
                for (int j = 0; j < points.Count; j++)
                {
                    if (mapLeft <= points[j].x && mapRight >= points[j].x
                        && mapDown <= points[j].y && mapUp >= points[j].y)
                    {
                        points.RemoveAt(j);
                        j--;
                    }
                }
                if (points.Count == beforeCount)
                {
                    maps[i].isFree = true;
                }
            }
        }

        if (points.Count > 0)
        {
            MapData freeData = GetFreeMap();
            freeData.isFree = false;

            int x, y;
            if (points[0].x >= 0)
            {
                for (x = 0; Mathf.Abs(points[0].x - rendWidth * x) > rendWidth / 2; x++) ;
            }
            else
            {
                for (x = 0; Mathf.Abs(points[0].x - rendWidth * x) > rendWidth / 2; x--) ;
            }

            if (points[0].y >= 0)
            {
                for (y = 0; Mathf.Abs(points[0].y - rendHeight * y) > rendHeight / 2; y++) ;
            }
            else
            {
                for (y = 0; Mathf.Abs(points[0].y - rendHeight * y) > rendHeight / 2; y--) ;
            }
            freeData.render.transform.position = new Vector2(x * rendWidth, y * rendHeight);
        }
    }

    private MapData GetFreeMap()
    {
        for (int i = 0; i < 4; i++)
        {
            if (maps[i].isFree)
            {
                return maps[i];
            }
        }
        return null;
    }

    public void ClearAll()
    {
        foreach (MapData map in maps)
        {
            GameObject.Destroy(map.render.gameObject);
        }
    }
}

public class MapData
{
    public SpriteRenderer render;
    public bool isFree;

    public MapData(SpriteRenderer render)
    {
        this.render = render;
        this.isFree = true;
    }
}

struct MaxData
{
    public float data;
    public Vector3 onwerLoc;

    public MaxData(float data, Vector3 onwerLoc)
    {
        this.data = data;
        this.onwerLoc = onwerLoc;
    }
}

//[Flags]
//public enum PosInNeed
//{
//    NONE        = 0b_0000_0000, // 无
//    NORTH       = 0b_0000_0001, // 北
//    NORTH_EAST  = 0b_0000_0010, // 东北
//    EAST        = 0b_0000_0100, // 东
//    SOUTH_EAST  = 0b_0000_1000, // 东南
//    SOUTH       = 0b_0001_0000, // 南
//    SOUTH_WEST  = 0b_0010_0000, // 西南
//    WEST        = 0b_0100_0000, // 西
//    NORTH_WEST  = 0b_1000_0000, // 西北
//}