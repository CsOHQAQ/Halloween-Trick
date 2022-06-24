using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QxFramework.Core;
public class CameraControl : MonoBehaviour
{
    private void Update()
    {
        if(ProcedureManager.Instance.Current is BattleProcedure)
        {
            this.transform.position = EntityManager.Instance.player.transform.position - new Vector3(0, 0, 10);
        }
        else
        {
            this.transform.position = new Vector3(0, 0, -10);
        }
    }
}
