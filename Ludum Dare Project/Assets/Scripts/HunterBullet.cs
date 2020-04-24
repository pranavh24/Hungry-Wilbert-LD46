using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBullet : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 7f;

    Rigidbody2D rb;
    Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameManager.current.player.gameObject;
        moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3f);
    }

    public GameObject bulletSpark;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform == GameManager.current.player.transform)
        {
            GameManager.current.playerBrain.TakeDamage(GameManager.current.playerBrain.damageAmount);
        }
        if (collision.gameObject.tag != "Hunter")
        {
            transform.GetChild(0).parent = null;
            Instantiate(bulletSpark, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        
    }
}
