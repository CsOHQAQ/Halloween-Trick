using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

[Category("Custom")]
[Description("巡逻")]
public class Patrol : ActionTask
{
    private EnemyBase self;
    private MoveGridState startGrid; //起点格子
    private MoveGridState aimGrid; //终点格子
    private MoveGridState[,] grids;
    private Stack<(int x, int y)> path = new Stack<(int x, int y)>();
    private List<MoveGridState> openList = new List<MoveGridState>();
    private List<MoveGridState> closeList = new List<MoveGridState>();

    protected override string info
    {
        get { return $"在地图内考虑焦虑值\n随机移动到某位置"; }
    }

    protected override void OnExecute()
    {
        self = agent.GetComponent<EnemyBase>();

        int xNum = GameWatcher.Instance.CurSituationManager.xNum;
        int yNum = GameWatcher.Instance.CurSituationManager.yNum;

        grids = new MoveGridState[xNum, yNum]; //创建记录格子代价信息的数组
        for (int i = 0; i < xNum; i++) //初始化该数组
        {
            for (int j = 0; j < yNum; j++)
            {
                grids[i, j] = new MoveGridState((i, j));
            }
        }
        openList.Clear();
        closeList.Clear();
        path.Clear();

        (int curX, int curY) = self.CurGrid.GetXYByString();
        startGrid = grids[curX - 1, curY - 1]; //起点格子就是现在在的格子,注意减一

        do //随机生成合法的终点格子
        {
            aimGrid = grids[Random.Range(1, xNum - 1), Random.Range(1, yNum - 1)]; //最外一圈锁定为格子？
        } while (!aimGrid.canPass);

        //开始寻路
        openList.Add(startGrid);
        while (openList.Count > 0)
        {
            // 寻找开启列表中的F最小的节点，如果F相同，选取H最小的
            MoveGridState currentNode = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                MoveGridState node = openList[i];
                if (node.TotalCost < currentNode.TotalCost || 
                    (node.TotalCost == currentNode.TotalCost && node.G < currentNode.G))
                {
                    currentNode = node;
                }
            }
            // 把当前节点从开启列表中移除，并加入到关闭列表中
            openList.Remove(currentNode);
            closeList.Add(currentNode);
            // 如果是目的节点，返回
            if (currentNode == aimGrid)
            {
                List<Vector3> Points = new List<Vector3>();

                while (aimGrid != null)
                {
                    Points.Insert(0, new Vector2(aimGrid.Position.x * 10, aimGrid.Position.y * 10));
                    path.Push(aimGrid.Position);
                    aimGrid = aimGrid.Parent;
                }

                Points.Insert(0, (Vector2)self.transform.position);
                //self.testLine = LineManager.Instance.SpawnLine(Points, 0.5f, "Line_Red");
                return;
            }
            // 搜索当前节点的所有相邻节点
            foreach (var node in GetNeighor(currentNode))
            {
                // 如果节点已在关闭列表中，跳出
                if (closeList.Contains(node))
                {
                    continue;
                }
                float F = currentNode.F + GetDistanceNodes(currentNode, node);
                // 如果新路径到相邻点的距离更短 或者不在开启列表中
                if (F < node.F || !openList.Contains(node))
                {
                    // 更新相邻点的F，G
                    node.F = F;
                    node.G = GetDistanceNodes(node, aimGrid);
                    // 设置相邻点的父节点为当前节点
                    node.Parent = currentNode;
                    // 如果不在开启列表中，加入到开启列表中
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
    }

    protected override void OnUpdate()
    {
        if (!self.isMove)
        {
            if (path.Count > 0 && !self.accidentStop)
            {
                (int x, int y) = path.Pop();
                Vector2 toward = new Vector2(x * 10, y * 10);
                self.MoveToGrid(x + 1,y + 1); //............
                self.Target = toward;
            }
            else
            {
                self.accidentStop = false; //需要手动设置此值
                LineManager.Instance.RemoveLine(self.testLine);
                EndAction();
            }
        }
    }

    //返回某节点附近的所有合法的节点
    private List<MoveGridState> GetNeighor(MoveGridState node)
    {
        int xNum = GameWatcher.Instance.CurSituationManager.xNum;
        int yNum = GameWatcher.Instance.CurSituationManager.yNum;
        List<MoveGridState> neighborList = new List<MoveGridState>();
        
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int tempX = node.Position.x + i;
                int tempY = node.Position.y + j;
                if (tempX < xNum - 1 && tempX > 0 && tempY > 0 && tempY - 1 < yNum //最外一圈锁定为障碍？
                    && grids[tempX, tempY].canPass)
                {
                    neighborList.Add(grids[tempX, tempY]);
                }
            }
        }
        return neighborList;
    }

    private float GetDistanceNodes(MoveGridState node1, MoveGridState node2)
    {
        int deltaX = Mathf.Abs(node1.Position.x - node2.Position.x);
        int deltaY = Mathf.Abs(node1.Position.y - node2.Position.y);

        return Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}