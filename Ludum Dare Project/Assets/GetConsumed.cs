using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetConsumed : MonoBehaviour
{
    private bool isEdible;
    public float foodVal = 10;
    AudioSource consumeSound;
    // Start is called before the first frame update

    private void Start()
    {
        isEdible = false;
        consumeSound = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public GameObject consumeFX;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform == GameManager.current.player.transform && isEdible)
        {
            GameManager.current.player.GetComponent<Player>().eatFood(foodVal);
            Instantiate(consumeFX, collision.transform);
            consumeSound.Play();
            transform.GetChild(0).parent = null;
            Destroy(gameObject);
        }
    }

    void BecomeEdible()
    {
        isEdible = true;
    }
}
