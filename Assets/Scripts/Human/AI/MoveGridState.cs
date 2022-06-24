using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGridState
{
    public (int x, int y) Position; //储存自己在数组中的位置
    public MoveGridState Parent = null; //储存父节点
    public float F = 0; //从起点到当前位置的代价(欧拉距离)
    public float G = 0; //从终点到当前位置的代价(曼哈顿距离)

    //总代价
    public float TotalCost => F + G + 
        GameWatcher.Instance.CurSituationManager.GridsInSubMap[Position.x, Position.y].threatLeve;

    //可否通过
    public bool canPass => 
        GameWatcher.Instance.CurSituationManager.GridsInSubMap[Position.x, Position.y].TotalCanPass;

    public MoveGridState((int x, int y) position)
    {
        Position = position;
    }
}

public static class ExtensionString
{
    //string转换成元组以方便操作
    public static (int x, int y) GetXYByString(this string p_sXY)
    {
        if (p_sXY == null)
            return (-1, -1);
        p_sXY = p_sXY.Replace("(", "").Replace(")", "");
        string[] s = p_sXY.Split(',');
        return (int.Parse(s[0]), int.Parse(s[1]));
    }
}
