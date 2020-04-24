using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape")) {
            Application.Quit();
        } else if(Input.GetKey("s")) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        } else if (Input.GetKey("c")) {
            SceneManager.LoadScene(4);
        }
    }
}
