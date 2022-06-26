using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public float Distance;
    public float FadeSpeed;
    public float MoveSpeed;

    private SpriteRenderer SpriteRenderer;
    private float TransValue=255;
    private bool Flag1 = true,
                         Flag2=true;
    private float MoveCount = 0;
    private void Awake()
    {
        SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        SpriteRenderer.color = new Color32((byte)255, (byte)255, (byte)255, (byte)TransValue);
        if (TransValue >= 0 && Flag1)
        {
            TransValue -= FadeSpeed * Time.deltaTime;
            if (TransValue < 0)
            {
                TransValue = 0;
                Flag1 = false;
            }
        }
        else if (TransValue <= 255 && !Flag1)
        {
            TransValue += FadeSpeed * Time.deltaTime;
            if (TransValue > 255)
            {
                TransValue = 255;
                Flag1 = true;
            }
        }

        if (MoveCount <= Distance&&Flag2)
        {
            this.transform.Translate(MoveSpeed * Time.deltaTime, 0,0);
            MoveCount += MoveSpeed * Time.deltaTime;
            if (MoveCount > Distance)
            {
                MoveCount = Distance;
                Flag2 = false;
            }
        }
        else if (MoveCount >=0 && !Flag2)
        {
            this.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0);
            MoveCount -= MoveSpeed * Time.deltaTime;
            if (MoveCount <0)
            {
                MoveCount = 0;
                Flag2 = true;
            }
        }
    }
}
