using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;

[Category("Custom")]
[Description("逃离")]
public class Escape : ActionTask
{
    public BBParameter<HumanBase> target; //威胁最大的目标

    private EnemyBase self;

    protected override string info
    {
        get { return $"逃离{target}"; }
    }

    protected override void OnExecute()
    {
        self = agent.GetComponent<EnemyBase>();

        int xNum = GameWatcher.Instance.CurSituationManager.xNum;
        int yNum = GameWatcher.Instance.CurSituationManager.yNum;
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
                if (tempX < xNum && tempX > 1 && tempY > 1 && tempY < yNum) //最外一圈锁定为格子？
                {
                    string curPos = $"({tempX},{tempY})";
                    GridInfo grid = GameObject.Find($"subMap/{curPos}").GetComponent<GridInfo>();
                    if (grid.InstanceCanPass(self))
                    {
                        float weight = GetDistanceWeight(curPos);
                        weight -= grid.threatLeve;
                        weights.Add(curPos, weight); //将该点位加入备选名单中
                    }
                }
            }
        }

        if (weights.Count == 0)
        {
            EndAction();
        }

        (string pos, float value) maxWeight = (null, float.NegativeInfinity);
        for (int i = 0; i < weights.Count; i++)
        {
            var weightEntry = weights.ElementAt(i);
            float curWeight = weightEntry.Value;

            (int x, int y) curPos = weightEntry.Key.GetXYByString();
            (int x, int y) nextPos = (curPos.x * 2 - curX, curPos.y * 2 - curY); //计算出再外一圈的点位
            GridInfo nextGrid = GameObject.Find($"subMap/({nextPos.x},{nextPos.y})").GetComponent<GridInfo>();
            if (!nextGrid.TotalCanPass) //不能通过增加10点
            {
                curWeight -= 10;
            }
            else //能通过增加该点威胁度的2/3
            {
                curWeight -= nextGrid.threatLeve * 2 / 3;
            }

            if (curWeight > maxWeight.value)
            {
                maxWeight.pos = weightEntry.Key;
                maxWeight.value = curWeight;
            }
        }

        (int xToMove, int yToMove) = maxWeight.pos.GetXYByString();
        Vector2 towards = new Vector2((xToMove - 1) * 10, (yToMove - 1) * 10);

        self.MoveToGrid(xToMove,yToMove);
        self.Target = towards;

        //self.testLine = LineManager.Instance.SpawnLine((Vector2)self.transform.position, towards, 0.5f, "Line_Red");
    }

    protected override void OnUpdate()
    {
        if (!self.isMove || self.accidentStop)
        {
            self.accidentStop = false;
            LineManager.Instance.RemoveLine(self.testLine);
            EndAction();
        }
    }

    //距离权重: |距离|平方
    private float GetDistanceWeight(string curPos)
    {
        (int curX, int curY) = curPos.GetXYByString();
        (int aimX, int aimY) = target.value.CurGrid.GetXYByString();
        int deltaX = Mathf.Abs(curX - aimX);
        int deltaY = Mathf.Abs(curY - aimY);

        return deltaX * deltaX + deltaY * deltaY;
    }
}