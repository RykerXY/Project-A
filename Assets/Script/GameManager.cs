using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    public Transform spawnPoint;
    public GameObject[] groundPrefabs; 
    public float groundHeight = 2.5f;
    private int groundCount = 0;
    public int grounStart = 0;
    private List<GameObject> groundList = new List<GameObject>();

    public void SpawnGround() {
    int index = Random.Range(0, groundPrefabs.Length);
    
    Vector3 spawnPointPosition = spawnPoint.position;
    Vector3 spawnPointPosition1 = spawnPoint.position;
    
    // สุ่มค่า X ของ platform อันแรก
    spawnPointPosition.x += Random.Range(-6f, 6f);
    
    // หา X ของ platform อันที่สองโดยให้มีระยะห่างที่เหมาะสม
    bool positionIsValid = false;
    int maxAttempts = 10; // ลองหาตำแหน่งใหม่ไม่เกิน 10 ครั้ง
    int attempt = 0;
    
    while (!positionIsValid && attempt < maxAttempts) {
        spawnPointPosition1.x = spawnPoint.position.x + Random.Range(-6f, 6f); 
        if (Mathf.Abs(spawnPointPosition1.x - spawnPointPosition.x) > 3f) { // ต้องห่างกันอย่างน้อย 2 หน่วย
            positionIsValid = true;
        }
        attempt++;
    }

    GameObject newGround = Instantiate(groundPrefabs[index], spawnPointPosition, Quaternion.identity);
    groundList.Add(newGround);
    
    GameObject newGround1 = Instantiate(groundPrefabs[index], spawnPointPosition1, Quaternion.identity);
    groundList.Add(newGround1);

    spawnPoint.position += Vector3.up * groundHeight;
}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(cam.transform.position.y);
        if(groundCount <= grounStart){
            SpawnGround();
            groundCount++;
        }

        for (int i = groundList.Count - 1; i >= 0; i--)
        {
            GameObject ground = groundList[i];
            if (ground.transform.position.y < cinemachineCamera.transform.position.y - 10f) {
                Debug.Log("Ground removed | Ground Count : " + groundCount);
                Destroy(ground);
                groundList.RemoveAt(i);
                groundCount--;
            }
        }
    }
}
