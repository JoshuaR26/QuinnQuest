using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame(){
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void rollCredits(){
        //Replace scene with credits scene once ready
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void quitGame(){
        if(UnityEditor.EditorApplication.isPlaying){
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else{
            Application.Quit();
        }
    }
}
