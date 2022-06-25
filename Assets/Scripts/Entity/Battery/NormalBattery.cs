using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBattery : Entity
{
    private float DetectionRadius;
    private float HPS;
    private LineRenderer DetectionCircle;
    private int PointCount = 360;//用于绘制检测圆的点数

    public void DrawDetectionCircle()
    {
        DetectionCircle.enabled = true;
        float SingleAngle = 360f / (PointCount - 1);
        Quaternion quaternion=new Quaternion();
        Vector3 CircleCenter = this.transform.position;
        DetectionCircle.positionCount = PointCount;
        for(int i = 0; i < PointCount; i++)
        {
            if (i != 0) quaternion = Quaternion.Euler(quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z + SingleAngle);
            Vector3 NextPoint = CircleCenter + quaternion * Vector3.down * DetectionRadius;
            DetectionCircle.SetPosition(i, NextPoint);
        }
    }//绘制检测圆

    public override void Init()
    {
        base.Init();
        CurHealth = MaxHealth = tab.GetFloat("Character", "NormalBattery", "Health");
        DetectionRadius = tab.GetFloat("Character", "NormalBattery", "DetectionRadius");
        HPS= tab.GetFloat("Character", "NormalBattery", "HPS");
        DetectionCircle = this.transform.Find("DetectionCircle").GetComponent<LineRenderer>();
        GetComponent<CircleCollider2D>().radius = DetectionRadius;
        weaponManager.Add("HandGun");
        DrawDetectionCircle();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 CollisionPosition=collision.transform.position;
        weaponManager.CurWeapon.Fire(CollisionPosition);
    }

    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, EntityManager.Instance.player.transform.position) <= EntityManager.Instance.player.HealBatteryRange)
        {
            CurHealth += EntityManager.Instance.player.HealBatterySpeed * Time.deltaTime;

        }
        else
        {

            CurHealth -= HPS * Time.deltaTime;
        }
    }
}
