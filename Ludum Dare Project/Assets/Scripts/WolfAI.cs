using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WolfAI : MonoBehaviour
{

    public bool died;
    AIPath astar;
    AIDestinationSetter destinationSetter;
    Animator anim;
    private static int gotHitAnimID;

    public float maxDistanceFromPlayer = 15f;

    SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        died = false;
        astar = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = GameManager.current.player;
        //First child should be the gameobject with the sprite and animator. 
        anim = transform.GetChild(0).GetComponent<Animator>();
        gotHitAnimID = Animator.StringToHash("GotHit");
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (astar.desiredVelocity.x < -.2)
        {
            TurnLeft();
        }
        else if(astar.desiredVelocity.x > .2)
        {
            TurnRight();
        }

        if ((GameManager.current.player.position - transform.position).sqrMagnitude > maxDistanceFromPlayer * maxDistanceFromPlayer)
        {
            GameManager.current.removeEnemies(1);
            Destroy(gameObject);
        }
    }

    public void TurnLeft()
    {
        sr.flipX = true;
    }

    public void TurnRight()
    {
        sr.flipX = false;
    }

    public void GetShot()
    {
        if (!died)
        {
            died = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CapsuleCollider2D>());
            astar.canSearch = false;
            astar.canMove = false;

            anim.SetTrigger(gotHitAnimID);
        }
    }
}
