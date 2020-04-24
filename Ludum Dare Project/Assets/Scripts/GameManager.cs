using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    
    [HideInInspector]
    public Transform player;
    public Player playerBrain;

    [HideInInspector]
    public static int numEnemies = 0;

    // Start is called before the first frame update

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Awake() {
        if (current == null) {
            current = this;
        } else {
            Destroy(gameObject);
        }
        
        
        player = GameObject.Find("Wilbert").transform;
    }

    private void Start()
    {
        playerBrain = current.player.GetComponent<Player>();
    }

    public void addEnemies(int enemies) {
        numEnemies += enemies;
    }

    public void removeEnemies(int enemies) {
        numEnemies -= enemies;
    }

    public int getEnemies(){
        return numEnemies;
    }

    // When game reloads, numEnemies set to 0. 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        numEnemies = 0;
    }
}
