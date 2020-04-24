using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    public GameObject pauseMenuTool;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(paused){
                resumeGame();
            }else{
                pauseGame();
            }

        }

        if(Time.timeScale == 1f){
            paused = false;
        } else /*if(Time.timeScale == 0f)*/{
            paused = true;
        }
    }

    public void resumeGame(){
        pauseMenuTool.SetActive(false);
        Time.timeScale = 1f;
        //paused = false;
    }
    public void pauseGame(){
        pauseMenuTool.SetActive(true);
        Time.timeScale = 0f;
        //paused = true;
    }

    public void goMenu(){
        SceneManager.LoadScene(0);
    }
    public void quit(){
        Application.Quit();
    }
}
