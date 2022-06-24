using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion;

[Category("Custom")]
[Description("逃离")]
public class CheckDistanceInGrid : ConditionTask
{
    [BlackboardOnly]
    public BBParameter<string> pos1;
    [BlackboardOnly]
    public BBParameter<string> pos2;
    public CompareMethod comparison = CompareMethod.EqualTo;
    public BBParameter<float> value;

    protected override string info
    {
        get { return $"{pos1}和{pos2}间的格子距离\n{OperationTools.GetCompareString(comparison)}{value.value}"; }
    }

    protected override bool OnCheck()
    {
        (int x1, int y1) = pos1.value.GetXYByString();
        (int x2, int y2) = pos2.value.GetXYByString();

        Vector2 v1 = new Vector2(x1, y1);
        Vector2 v2 = new Vector2(x2, y2);
        float d = Vector2.Distance(v1, v2);
        
        return OperationTools.Compare(d, value.value, comparison, 0f);
    }
}