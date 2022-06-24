using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class PlayerEntity : Entity
{

    public override void Init()
    {
        base.Init();
        MaxHealth = tab.GetFloat("Character", "Player", "Health");
        MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

    }

    private void Awake()
    {
        base.Init();

        MaxHealth = tab.GetFloat("Character", "Player", "Health");
        MoveSpeed = tab.GetFloat("Character", "Player", "MoveSpeed");

      
    }

    private void Start()
    {
        this.GetComponent<WeaponManager>().Add("MachineGun");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * MoveSpeed*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector2.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (Vector3)Vector2.up * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (Vector3)Vector2.down * MoveSpeed * Time.deltaTime;
        }

        this.GetComponent<WeaponManager>().CurWeapon.Fire(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
