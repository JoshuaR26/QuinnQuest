using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class anyEnd : MonoBehaviour
{
    [SerializeField] private float endDelay = 1f;
    [SerializeField] private float endSize;
    [SerializeField] private bool autoNext = false;
    [SerializeField] private string Scene;
    [SerializeField] private string filePath = "";
    [SerializeField] private SpriteMask spriteMask;
    [SerializeField] private bool shrinkMask = false;

    private bool isEnd = false;
    private VideoPlayer videoPlayer;
    private bool shrink = false;

    // Added for shrink logic
    private float elapsed = 0f;
    private Vector3 startScale;

    void Start()
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            GameObject camera = GameObject.Find("Main Camera");
            videoPlayer = camera.AddComponent<VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.url = filePath;
            videoPlayer.isLooping = true;
            videoPlayer.loopPointReached += OnLoopPointReached;
        }
    }

    void OnLoopPointReached(VideoPlayer videoPlayer)
    {
        if (autoNext)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadSceneAsync(Scene);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (shrinkMask)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }

            if (spriteMask != null)
            {
                shrink = true;
                elapsed = 0f;
                startScale = spriteMask.transform.localScale;
            }
        }

        else if (!string.IsNullOrEmpty(filePath) && videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void Update()
    {
        if (shrink && spriteMask != null)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / endDelay);
            Vector3 targetScale = new Vector3(endSize, endSize, startScale.z);
            spriteMask.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            if (t >= 1f)
            {
                Debug.Log("Shrink complete");
                shrink = false;
                isEnd = true;
            }
        }
        if (isEnd)
        {
            if (!string.IsNullOrEmpty(filePath) && videoPlayer != null)
            {
                videoPlayer.Play();
            }
            else if (autoNext)
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadSceneAsync(Scene);
            }
        }
    }
}
