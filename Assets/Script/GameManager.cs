using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;

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

        spawnPointPosition.x += Random.Range(-6f, 6f);

        GameObject newGround = Instantiate(groundPrefabs[index], spawnPointPosition, Quaternion.identity);
        groundList.Add(newGround);

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
