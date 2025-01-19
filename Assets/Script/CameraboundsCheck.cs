using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraboundsCheck : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    private Bounds cameraBounds;
    public float extraMargin = 0.5f;
    void Start()
    {
        
    }

    void Update()
    {
        if (cinemachineCamera != null) {
            UpdateCameraBounds();
            CheckIfOutOfBounds();
        }
    }
    void UpdateCameraBounds() {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        cameraBounds = new Bounds(cam.transform.position, new Vector3(camWidth, camHeight, 0));
    }

    void CheckIfOutOfBounds() {
        if (transform.position.y < cameraBounds.min.y - extraMargin) {
            GameOver();
        }
    }

    void GameOver() {
        Debug.Log("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
