using UnityEngine;

public class PlayZone : MonoBehaviour
{
    public Transform player; // ตัวละครที่ต้องให้ Collider ติดตาม
    public GameManager gameManager;
    private float lastPlayerY; // ตำแหน่ง Y ล่าสุดของ Player
    public float yMove = 0f;
    void Start()
    {
        if (player != null) {
            if (player != null) {
            lastPlayerY = player.position.y; // กำหนดค่าเริ่มต้น Y ของ Player
        }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            // เช็กว่าผู้เล่นขยับขึ้นหรือไม่
            if (player.position.y > lastPlayerY) {
                // ขยับ Collider ขึ้นตาม Player
                transform.position = new Vector3(transform.position.x, transform.position.y + yMove, transform.position.z);
            }

            // อัปเดตค่า Y ล่าสุดของ Player
            lastPlayerY = player.position.y;
        }
    }
}
