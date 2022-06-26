using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    public float MaxValue;
    public float MinValue;
    public float ChangeSpeed;

    private Light2D Light2D;
    private bool flag = true;
    void Start()
    {
        Light2D = this.GetComponent<Light2D>();
    }
    void Update()
    {
        if (Light2D != null) 
        {
            if (Light2D.intensity <= MaxValue && flag)
            {
                Light2D.intensity += ChangeSpeed * Time.deltaTime;
            }
            else if (flag)
            {
                flag = false;
            }

            if (Light2D.intensity >= MinValue && !flag)
            {
                Light2D.intensity -= ChangeSpeed * Time.deltaTime;
            }
            else if (!flag)
            {
                flag = true;
            }
        }
    }
}
