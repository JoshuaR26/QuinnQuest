using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector3 targetPosition = new Vector3(-221.7f, 9.3f, 0f);
    [SerializeField] float targetZoom = 2f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float acceleration = 1.05f;
    [SerializeField] string nextSceneName = "SampleScene"; // Scene to load

    private bool startZoom = false;
    private bool hasReachedPosition = false;
    private bool hasZoomedIn = false;

    void Start()
    {
        if (cam == null)
            cam = Camera.main; // Use main camera if not assigned
    }

    void Update()
    {
        if (startZoom)
        {
            Vector3 currentPosition = cam.transform.position;
            
            float positionDistance = Vector3.Distance(currentPosition, targetPosition);
            float zoomDistance = Mathf.Abs(cam.orthographicSize - targetZoom);

            if(!hasReachedPosition || !hasZoomedIn){
                if (positionDistance > 0.1f)
                {
                    cam.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                    moveSpeed *= acceleration;
                }
                else
                {
                    hasReachedPosition = true;
                }


                if (zoomDistance > 0.1f)
                {
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
                    zoomSpeed *= acceleration; 
                }
                else if (zoomDistance <= 0.1f)
                {
                    cam.orthographicSize = targetZoom; 
                    hasZoomedIn = true;
                }
            }
            else{
                SceneManager.LoadSceneAsync(nextSceneName);
            }
        }
    }

    public void PlayGame()
    {
        startZoom = true; // Start zooming and moving when Play is clicked
    }

    public void RollCredits()
    {
        SceneManager.LoadSceneAsync("SampleScene"); // Replace with credits scene later
    }

    public void QuitGame()
    {
        if(UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
