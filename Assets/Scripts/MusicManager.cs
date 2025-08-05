using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float startDelay = 9f;              // Delay after intro scene starts
    public float fadeOutDuration = 2f;         // Duration of fade when stopping
    public float startAtTime = 10f;            // Start song at 10 seconds
    public string stopOnButtonScene = "Practice";

    private static MusicManager instance;

    void Awake()
    {
        // Singleton pattern to persist through scenes
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(PlayMusicAfterDelay());
    }

    IEnumerator PlayMusicAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);

        if (audioSource != null)
        {
            audioSource.time = startAtTime;  // ⏱ Start from 0:10 in the song
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not assigned on MusicManager.");
        }
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutAndStop());
    }

    IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;

        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset in case reused later
    }
}
