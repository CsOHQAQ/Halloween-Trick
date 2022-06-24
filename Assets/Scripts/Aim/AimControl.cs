using UnityEngine;
public class AimControl : MonoBehaviour
{
    public float CurrentSpread;
    public bool isFollowing = true;

    public void Init()
    {
        Debug.Log("Init Aimer");
        isFollowing = true;
    }

    private void Update()
    {
            AimFollow();
    }
    private void AimFollow()
    {
        Vector2 MousePos = new Vector2();
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = MousePos;
    }
}
