using UnityEngine;

public class BackgroundSpawner1 : MonoBehaviour
{
    [Header("Background Change When?")]
    public int Score_is = 10;
    [Header("Background Prefabs")]
    public GameObject backgroundPrefab1;
    public GameObject backgroundPrefab2;
    [Header("Background Spawning")]
    public float spawnY = 10f; // จุดที่สร้าง Background ใหม่
    public float deleteY = -10f;
    public float offsetY = 10f;

    private GameObject useBackground;
    private Transform player;

    private bool isFading = false;
    public Score score;
    private static float lastSpawnY;

    void Start()
    {
        useBackground = backgroundPrefab1;
        player = Camera.main.transform;
        offsetY = backgroundPrefab1.GetComponent<SpriteRenderer>().bounds.size.y/2;

        lastSpawnY = spawnY; // เริ่มต้นการสร้างจาก spawnY
    }

    void Update()
    {
        if(score.GetScore() > Score_is) {
            useBackground = backgroundPrefab2;
            isFading = true;
        }

        // ถ้าผู้เล่นขึ้นไปสูงพอ สร้าง Background ใหม่
        if (player.position.y > lastSpawnY-20)
        {
            SpawnBackground();
            lastSpawnY += offsetY; // อัปเดตตำแหน่งของพื้นหลังใหม่
        }

        // ลบพื้นหลังที่ต่ำกว่ากล้องมากเกินไป
        foreach (Transform bg in transform)
        {
            if (bg.position.y < player.position.y + deleteY)
            {
                Destroy(bg.gameObject);
            }
        }
    }

    void SpawnBackground()
    {
        Debug.Log("Background Spawned at Y: " + lastSpawnY);
        Vector3 spawnPos = new Vector3(0, lastSpawnY, 0); // ใช้ lastSpawnY เป็นตำแหน่งเริ่มต้น
        // Spawn Background ใหม่ และอัปเดต lastSpawnY
        GameObject newBg = Instantiate(useBackground, spawnPos, Quaternion.identity, transform);

        lastSpawnY += offsetY; // อัปเดต lastSpawnY สำหรับพื้นหลังที่ถูกสร้างใหม่
    }

    public bool getIsFading()
    {
        return isFading;
    }
}
