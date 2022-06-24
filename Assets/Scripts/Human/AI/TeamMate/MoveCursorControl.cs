using Chronos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursorControl : MonoBehaviour
{
    public RigidbodyTimeline2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Timeline>().rigidbody2D;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos), transform.position);
        Vector2 toward = ((Vector2)Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos) - (Vector2)transform.position).normalized;
        
        body.velocity = 50 * distance * toward; //速度和距离成正比，从而避免超过终点

        if (distance < 0.5)
        {
            body.velocity = Vector2.zero;
        }
    }
}
