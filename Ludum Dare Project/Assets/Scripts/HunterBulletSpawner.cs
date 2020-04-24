using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBulletSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject bullet;

    public float fireDelay;
    float nextFire;

    

    void Start()
    {
        nextFire = Time.time;
    }

    void Update()
    {
        check();
    }

    void check() { 
        if (Time.time > nextFire)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFire = Time.time + fireDelay;
        }
    }
}
