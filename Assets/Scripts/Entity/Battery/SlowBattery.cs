using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBattery : Entity
{
    private float DetectionRadius,
                         PowerRate,
                         HPS;
    private LineRenderer DetectionCircle;
    private int PointCount = 360;//用于绘制检测圆的点数
    private List<GameObject> collisionPool;

    public void DrawDetectionCircle()
    {
        DetectionCircle.enabled = true;
        float SingleAngle = 360f / (PointCount - 1);
        Quaternion quaternion = new Quaternion();
        Vector3 CircleCenter = this.transform.position;
        DetectionCircle.positionCount = PointCount;
        for (int i = 0; i < PointCount; i++)
        {
            if (i != 0) quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z + SingleAngle);
            Vector3 NextPoint = CircleCenter + quaternion * Vector3.down * DetectionRadius;
            DetectionCircle.SetPosition(i, NextPoint);
        }
    }//绘制检测圆

    public override void Init()
    {
        base.Init();
        data.CurHealth = data.MaxHealth = tab.GetFloat("Character", "SlowBattery", "Health");
        DetectionRadius = tab.GetFloat("Character", "SlowBattery", "DetectionRadius");
        PowerRate = tab.GetFloat("Character", "SlowBattery", "PowerRate");
        HPS = tab.GetFloat("Character", "SlowBattery", "HPS");
        DetectionCircle = this.transform.Find("DetectionCircle").GetComponent<LineRenderer>();
        GetComponent<CircleCollider2D>().radius = DetectionRadius;
        DrawDetectionCircle();
        collisionPool = new List<GameObject>();
    }
    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, EntityManager.Instance.player.transform.position) <= EntityManager.Instance.player.HealBatteryRange)
        {
            Data.CurHealth += EntityManager.Instance.player.HealBatterySpeed * Time.deltaTime;

        }
        else
        {

            Data.CurHealth -= HPS * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionPool.Add(collision.gameObject);
        ChildBase childBase = collision.GetComponent<ChildBase>();
        if (childBase != null) childBase.data.MoveSpeed *= PowerRate;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ChildBase childBase = collision.GetComponent<ChildBase>();
        if (childBase != null) childBase.data.MoveSpeed /= PowerRate;
        collisionPool.Remove(collision.gameObject);
    }

}
