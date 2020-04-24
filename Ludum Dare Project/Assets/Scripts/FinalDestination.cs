using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDestination : MonoBehaviour
{
    private bool hasWilbert;
    private bool hasJr;

    private void Start()
    {
        hasWilbert = false;
        hasJr = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name + "entered");
        if (collision.name == "Wilbert")
        {
            hasWilbert = true;
        }
        if (collision.name == "WilbertJr")
        {
            hasJr = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print(collision.name + "exited");
        if (collision.name == "Wilbert")
        {
            hasWilbert = false;
        }
        if (collision.name == "WilbertJr")
        {
            hasJr = false;
        }
    }

    private void Update()
    {
        if (hasWilbert && hasJr) {
            SceneManager.LoadScene(4);
        }
    }
}
