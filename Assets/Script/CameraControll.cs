using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public GameObject MainCamera;
    public Vector3 cameraPosition;

    private void OnTriggerEnter2D(Collider2D other) {
        MainCamera.transform.position = cameraPosition;
        Debug.Log("Collision Entered");
    }
}
