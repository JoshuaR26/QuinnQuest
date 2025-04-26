using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGmusic : MonoBehaviour
{
    public static BGmusic instance;

    private AudioSource audioSource;
    private bool isFadingOut = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Ending") // your ending scene name
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    IEnumerator FadeOutAndDestroy()
    {
        if (audioSource == null) yield break;

        isFadingOut = true;
        float startVolume = audioSource.volume;

        // Fade over 1 second
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
