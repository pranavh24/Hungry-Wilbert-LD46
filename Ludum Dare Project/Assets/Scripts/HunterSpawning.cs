using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSpawning : MonoBehaviour
{
    public GameObject hunter;
    public GameObject wolf;
    Camera cam;
    public float maxDistanceFromPlayerToSpawn;

    private GameManager manager;

    void Start()
    {
        StartCoroutine(spawn());
        cam = Camera.main;
        manager = GameManager.current;
    }

    IEnumerator spawn()
    {
        yield return new WaitForSeconds(2f);

        //print("There are " + numEnemies + " enemies.");
        

        while(true)
        {
            yield return new WaitUntil(() => Mathf.Abs(cam.WorldToViewportPoint(transform.position).x - 0.5f) > 0.51f &&
                            Mathf.Abs(cam.WorldToViewportPoint(transform.position).y - 0.5f) > 0.51f && 
                            (transform.position - GameManager.current.player.position).sqrMagnitude < maxDistanceFromPlayerToSpawn * maxDistanceFromPlayerToSpawn);

            

            if(manager.getEnemies() < 10) {
                Instantiate(
                    hunter,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity
                );

                manager.addEnemies(1);
            }

            if(manager.getEnemies() < 10) {
                Instantiate(
                    wolf,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    Quaternion.identity
                );


                manager.addEnemies(1);
            }

            yield return new WaitForSeconds(10f);
        }
    }

    
}
