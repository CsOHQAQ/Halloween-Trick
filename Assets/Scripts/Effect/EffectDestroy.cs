using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    public float LifeTime = 5;
    void Start()
    {
        Destroy(this.gameObject,LifeTime);
    }

}
