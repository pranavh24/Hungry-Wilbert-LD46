using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject food;
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject phlegmSpolsion;

    private GameManager manager;
    AudioSource splatterSound;
    void Start() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
        rb.velocity = transform.right * speed;
        splatterSound = transform.GetChild(1).GetComponent<AudioSource>();
        manager = GameManager.current;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {
        if (hitInfo.name.StartsWith("Hu"))
        {
            Instantiate(phlegmSpolsion, transform.position, Quaternion.identity);
            hitInfo.GetComponent<HunterAI>().GetShot();

            manager.removeEnemies(1);
            DestroyPreserveTrail();
        }
        else if (hitInfo.name.StartsWith("Wo"))
        {
            Instantiate(phlegmSpolsion, transform.position, Quaternion.identity);

            hitInfo.GetComponent<WolfAI>().GetShot();

            manager.removeEnemies(1);
            DestroyPreserveTrail();

        }
        else if (hitInfo.name.StartsWith("Wi") && hitInfo.transform != GameManager.current.player)
        {
            print("It didn't hit Wilbert, it hit Wilbert Jr.");
            // If transform is not the player and it starts with Wi, it's gotta be Wilbert Jr.
            hitInfo.transform.GetComponent<WilbertJr>().GetFed();
            DestroyPreserveTrail();

        }
        else if (!hitInfo.name.StartsWith("Wi") && !hitInfo.name.StartsWith("Fo"))
        {
            print("It hit Wilbert");
            Instantiate(phlegmSpolsion, transform.position, Quaternion.identity);
            DestroyPreserveTrail();
        }

        // Used StartsWith and first two letters to save on computation time
    }

    void DestroyPreserveTrail()
    {
        splatterSound.Play();
        transform.GetChild(1).parent = null;
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
        
    }

    void OnBecameInvisible() {
         Destroy(gameObject);
    }

}
