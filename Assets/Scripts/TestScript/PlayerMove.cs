using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 3;

    private WeaponBase NowsGun;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(new Vector3(-MoveSpeed*Time.deltaTime, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(new Vector3(MoveSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(new Vector3(0, MoveSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(new Vector3(0, -MoveSpeed * Time.deltaTime));
        }
        //粪移动

        if (Input.GetMouseButtonDown(0))
        {
            
        }
        if (Input.GetMouseButton(0))
        {
            
        }

    }
}
