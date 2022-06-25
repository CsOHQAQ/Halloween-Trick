using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public float LifeTime = 5;
    void Start()
    {
        this.transform.rotation = Quaternion.AngleAxis(Random.Range(-45,45), Vector3.forward);
        Destroy(this.gameObject,LifeTime);
    }

}
