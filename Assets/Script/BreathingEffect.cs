using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BreathingEffect : MonoBehaviour
{
    public float fadeDuration = 1f; // ระยะเวลาในการจางและกลับมา
    public float fadeInterval = 2f; // เวลาระหว่างการจางและกลับมาครั้งต่อไป
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>(); // ถ้าไม่มี CanvasGroup ให้เพิ่ม
        }
        StartCoroutine(FadeInOut());
    }
    void Update()
    {
        OnSpacebarPressed();
    }

    IEnumerator FadeInOut()
    {
        while (true)
        {
            // ทำให้ข้อความค่อยๆ จาง
            yield return StartCoroutine(FadeTo(0f, fadeDuration));
            // ทำให้ข้อความกลับมาเต็มที่
            yield return StartCoroutine(FadeTo(1f, fadeDuration));
            yield return new WaitForSeconds(fadeInterval); // รอเวลา
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        // ทำให้แน่ใจว่าได้ค่า targetAlpha ที่แน่นอน
        canvasGroup.alpha = targetAlpha;
    }

    public void OnSpacebarPressed()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
        
    }
}
