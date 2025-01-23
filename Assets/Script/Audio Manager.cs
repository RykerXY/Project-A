using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip BGM1;
    public AudioClip BGM2;
    public AudioClip BGM3;
    private static AudioManager instance;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // ป้องกัน AudioManager ซ้ำซ้อน
        }
    }
    
    public void PlayBGM1()
    {
        audioSource.clip = BGM1;
        audioSource.Play();
    }
    public void PlayBGM2()
    {
        audioSource.clip = BGM2;
        audioSource.Play();
    }
    public void PlayBGM3()
    {
        audioSource.clip = BGM3;
        audioSource.Play();
    }
    
    // Utilities
    public void StopAudio()
    {
        audioSource.Stop();
    }
}
