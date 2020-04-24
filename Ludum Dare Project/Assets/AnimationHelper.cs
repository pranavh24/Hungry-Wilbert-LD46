using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public GameObject food;
    // All objects that die should have sprites as their children; this will be run by the sprite which 
    // then calls destroy on its parent
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySelf()
    {
        StartCoroutine(FadeAndDestroy());
    }

    float timeToDestroy = 1f;

    void SpawnFood()
    {
        Instantiate(food, transform.position, Quaternion.identity);
    }

    IEnumerator FadeAndDestroy()
    {

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Color current = sr.color;
        Color final = new Color(current.r, current.g, current.b, 0);
        float t = 0;
        while(t < 1)
        {
            sr.color = Color.Lerp(current, final, t);
            t += Time.fixedDeltaTime / timeToDestroy;
            yield return new WaitForFixedUpdate();
        }

        Destroy(transform.parent.gameObject);
        yield return null;
    }

    public void DestroySelfImmediately()
    {
        Destroy(gameObject);
    }
}
