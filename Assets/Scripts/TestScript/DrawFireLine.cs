using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFireLine : MonoBehaviour
{
    LineRenderer LineRenderer;
    private void Start()
    {
        LineRenderer = this.GetComponent<LineRenderer>();
    }
    void Update()
    {
        LineRenderer.SetPosition(0, this.transform.parent.GetChild(0).transform.position);
        LineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
