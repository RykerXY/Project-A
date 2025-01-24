using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform target; // ตัวละครหลักที่ใช้เป็น reference
    public float parallaxFactor = 0.5f; // ค่าความเร็วในการเลื่อนของพื้นหลัง

    private float startY;

    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }
        startY = target.position.y;
    }

    void Update()
    {
        float deltaY = target.position.y - startY;
        transform.position = new Vector3(transform.position.x, startY + deltaY * parallaxFactor, transform.position.z);
    }
}
