using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;

[Category("Custom")]
[Description("周旋")]
public class Tug : ActionTask
{
    public BBParameter<HumanBase> target; //威胁最大的目标
    public BBParameter<float> safeDistance; //安全距离

    private EnemyBase self;
    private bool canMove; //如果找不到地方去移动，则原地停止 timeToWait 秒
    private float timeToWait = 0.5f;
    private float counter;

    protected override string info
    {
        get { return $"和{target}周旋\n安全距离为{safeDistance.value}"; }
    }

    protected override void OnExecute()
    {
        self = agent.GetComponent<EnemyBase>();

        int xNum = GameWatcher.Instance.CurSituationManager.xNum;
        int yNum = GameWatcher.Instance.CurSituationManager.xNum;
        Dictionary<string, float> weights = new Dictionary<string, float>(); //用于记录位置和对应权重
        (int curX, int curY) = self.CurGrid.GetXYByString();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int tempX = curX + i;
                int tempY = curY + j;
                if (tempX < xNum && tempX > 1 && tempY > 1 && tempY < yNum) //最外一圈锁定为障碍？
                {
                    string curPos = $"({tempX},{tempY})";
                    GridInfo grid = GameWatcher.Instance.CurSituationManager.GridsInSubMap[tempX - 1, tempY - 1];
                    if (grid.InstanceCanPass(self))
                    {
                        float weight = grid.threatLeve;
                        weight += GetDistanceWeight(curPos);
                        weights.Add(curPos, weight); //将该点位加入备选名单中
                    }
                }
            }
        }

        if (weights.Count == 0)
        {
            canMove = false;
            counter = 0;
            return;
        }
        else
        {
            canMove = true;
        }

        (string pos, float value) minWeight = (null, float.PositiveInfinity);
        for (int i = 0; i < weights.Count; i++)
        {
            var weightEntry = weights.ElementAt(i);
            float curWeight = weightEntry.Value;

            (int x, int y) curPos = weightEntry.Key.GetXYByString();
            (int x, int y) nextPos = (curPos.x * 2 - curX, curPos.y * 2 - curY); //计算出再外一圈的点位
            GridInfo nextGrid = GameWatcher.Instance.CurSituationManager.GridsInSubMap[nextPos.x - 1, nextPos.y - 1];
            if (!nextGrid.TotalCanPass) //不能通过增加10点
            {
                curWeight += 10;
            }
            else //能通过增加该点威胁度的2/3
            {
                curWeight += nextGrid.threatLeve * 2 / 3;
            }

            if (curWeight < minWeight.value)
            {
                minWeight.pos = weightEntry.Key;
                minWeight.value = curWeight;
            }
        }

        (int xToMove, int yToMove) = minWeight.pos.GetXYByString();
        Vector2 towards = new Vector2((xToMove - 1) * 10, (yToMove - 1) * 10);

        //self.MoveTo(towards);
        self.MoveToGrid(xToMove, yToMove);

        //self.testLine = LineManager.Instance.SpawnLine((Vector2)self.transform.position, towards, 0.5f, "Line_Red");
    }

    protected override void OnUpdate()
    {
        self.Target = target.value.transform.position;

        if (canMove)
        {
            if (!self.isMove || self.accidentStop)
            {
                self.accidentStop = false;
                LineManager.Instance.RemoveLine(self.testLine);
                EndAction();
            }
        }
        else
        {
            if (counter < timeToWait)
            {
                counter += self.time.deltaTime;
            }
            else
            {
                EndAction();
            }
        }
    }

    //距离权重: |安全距离-实际距离|平方
    private float GetDistanceWeight(string curPos)
    {
        (int curX, int curY) = curPos.GetXYByString();
        (int aimX, int aimY) = target.value.CurGrid.GetXYByString();
        int deltaX = Mathf.Abs(curX - aimX);
        int deltaY = Mathf.Abs(curY - aimY);

        return Mathf.Pow(safeDistance.value - Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY), 2);
    }
}