using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip jumpSoundl;
    public AudioClip chargJumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip landingSound;
    public AudioClip dazzlingSound;
    public AudioClip bounceSound;

    private void Awake() {
        audioSource.clip = walkSound;
    }
    public void playOnLoad()
    {
        Debug.Log("CharacterAudio is Loaded!");
    }

    public void PlayWalkSound()
    {
        audioSource.clip = walkSound;
        audioSource.Play();
    }

    public void PlayJumpSound()
    {
        audioSource.clip = jumpSoundl;
        audioSource.Play();
    }

    public void PlayChargeJumpSound()
    {
        audioSource.clip = chargJumpSound;
        audioSource.Play();
    }

    public void PlayDoubleJumpSound()
    {
        audioSource.clip = doubleJumpSound;
        audioSource.Play();
    }
    public void PlayLandingSound()
    {
        audioSource.clip = landingSound;
        audioSource.Play();
    }

    public void PlayDazzlingSound()
    {
        audioSource.clip = dazzlingSound;
        audioSource.Play();
    }

    public void PlayBounceSound()
    {
        audioSource.clip = bounceSound;
        audioSource.Play();
    }

    // Utilities
    public void StopAudio()
    {
        audioSource.Stop();
    }
    public void PauseAudio()
    {
        audioSource.Pause();
    }
    public void UnPauseAudio()
    {
        audioSource.UnPause();
    }
    public float getLength()
    {
        float length = audioSource.clip.length;
        return length;
    }
    public float getTime()
    {
        float time = audioSource.time;
        return time;
    }
    public bool isPlaying()
    {
        bool isPlaying = audioSource.isPlaying;
        return isPlaying;
    }
    public string getClipName()
    {
        string clipName = audioSource.clip.name;
        return clipName;
    }
    public void checkAudioClip(AudioClip audioClip)
    {
        if(audioClip == null)
        {
            audioClip = walkSound;
        }
    }
}
