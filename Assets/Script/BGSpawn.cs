using UnityEngine;

public class BGSpawn : MonoBehaviour
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
        offsetY = backgroundPrefab1.GetComponent<SpriteRenderer>().bounds.size.y / 2f;

        lastSpawnY = spawnY; // เริ่มต้นการสร้างจาก spawnY

        for (int i = 0; i < 3; i++)  
        {
            SpawnBackground();
        }
    }

    void Update()
    {
        if(score.GetScore() > Score_is) {
            useBackground = backgroundPrefab2;
            isFading = true;
        }

        if(!isFading){
        // ถ้าผู้เล่นขึ้นไปสูงพอ สร้าง Background ใหม่
            if (player.position.y > lastSpawnY + offsetY)
            {
                SpawnBackground();
                lastSpawnY += offsetY;
            }
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
        Debug.Log("Background Spawned at Y: " + spawnY);
        Vector3 spawnPos = new Vector3(0, spawnY, 0);
        // Spawn Background ใหม่ และอัปเดต spawnY
        GameObject newBg = Instantiate(useBackground, spawnPos, Quaternion.identity, transform);

        spawnY += offsetY; // อัปเดต spawnY สำหรับพื้นหลังที่ถูกสร้างใหม่
    }

    public bool getIsFading()
    {
        return isFading;
    }
}
