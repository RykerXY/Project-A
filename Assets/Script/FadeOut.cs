using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeSpeed = 1f;
    private bool isFading = false;
    private BackgroundSpawner backgroundSpawner;

    void Start()
    {
        backgroundSpawner = GameObject.Find("BackgroundManager").GetComponent<BackgroundSpawner>();
    }

    void Update()
    {
        isFading = backgroundSpawner.getIsFading();
        if (isFading){
        Debug.Log("Is Fading");
        // ลดค่า alpha ใน Color ของ Sprite Renderer
        Color color = spriteRenderer.color;
        color.a -= fadeSpeed * Time.deltaTime; // ลด alpha
        spriteRenderer.color = color;

        // ป้องกันไม่ให้ alpha ต่ำกว่า 0
        if (color.a < 0)
        {
            color.a = 0;
            spriteRenderer.color = color;
        }
        }
    }
}
