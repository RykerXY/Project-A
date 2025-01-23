using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float speed = 2f;
    public float resetPosition = 10f; // ตำแหน่งที่พื้นหลังควรเริ่มใหม่

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // เลื่อนพื้นหลังขึ้น
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // เช็คว่าพื้นหลังหลุดจากขอบแล้วหรือยัง
        if (transform.position.y >= resetPosition)
        {
            transform.position = startPosition;
        }
    }
}
