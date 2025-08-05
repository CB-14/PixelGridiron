using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject tapToStartUI;
    public CanvasGroup tapToStartGroup;
    public TextMeshProUGUI tapToStartText;

    public string sceneToLoad = "MainMenu";

    private bool waitingForInput = false;
    private Coroutine blinkCoroutine;
    public float blinkSpeed = 3f; // Speed of blink fade

    void Start()
    {
        if (videoPlayer != null)
        {
            Debug.Log("Start: Hiding tapToStartUI and preparing video.");

            tapToStartUI.SetActive(false);
            tapToStartGroup.alpha = 0f;
            videoPlayer.targetCameraAlpha = 1f;

            videoPlayer.loopPointReached += OnVideoEnd;

            videoPlayer.Prepare();
            StartCoroutine(PlayWhenReady());

            // Optional: debug fallback if video fails
            StartCoroutine(TestShowUIAfterDelay(7f));
        }
        else
        {
            Debug.LogWarning("VideoPlayer reference is missing!");
        }
    }

    IEnumerator PlayWhenReady()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Video prepared, starting playback.");
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video ended, delaying UI show slightly.");
        StartCoroutine(DelayedShowTap(0.2f));
    }

    IEnumerator DelayedShowTap(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ShowTapAndWait());
    }

    IEnumerator ShowTapAndWait()
    {
        tapToStartUI.SetActive(true);
        Debug.Log("TapToStart UI activated, starting fade in.");

        yield return StartCoroutine(FadeCanvasGroup(tapToStartGroup, 0f, 1f, 1f));

        videoPlayer.targetCameraAlpha = 0f;
        waitingForInput = true;

        if (tapToStartText != null)
        {
            blinkCoroutine = StartCoroutine(BlinkTextSmooth());
        }
    }

    IEnumerator TestShowUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!tapToStartUI.activeSelf && !waitingForInput)
        {
            Debug.LogWarning("TestShowUIAfterDelay: Forcing tapToStartUI active after delay.");
            tapToStartUI.SetActive(true);
            StartCoroutine(FadeCanvasGroup(tapToStartGroup, 0f, 1f, 1f));
            waitingForInput = true;

            if (tapToStartText != null)
            {
                blinkCoroutine = StartCoroutine(BlinkTextSmooth());
            }
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;
        cg.alpha = start;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        cg.alpha = end;
    }

    // Smooth blink coroutine using alpha fade
    IEnumerator BlinkTextSmooth()
    {
        Color baseColor = tapToStartText.color;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color newColor = baseColor;
            newColor.a = Mathf.Lerp(0.2f, 1f, t); // Fade between 20% and 100% opacity
            tapToStartText.color = newColor;

            yield return null;
        }
    }

    void Update()
    {
        if (waitingForInput && Input.anyKeyDown)
        {
            Debug.Log("Input detected, loading scene: " + sceneToLoad);

            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);

            SceneManager.LoadScene(sceneToLoad);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool newState = !tapToStartUI.activeSelf;
            tapToStartUI.SetActive(newState);
            Debug.Log("Manual toggle of tapToStartUI: " + newState);
        }

        if (!tapToStartUI.activeSelf && waitingForInput)
        {
            Debug.LogWarning("tapToStartUI got disabled unexpectedly!");
        }
    }

    void OnDisable()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);
    }
}
