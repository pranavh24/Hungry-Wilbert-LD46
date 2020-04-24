using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAI : MonoBehaviour
{

    [SerializeField]
    public GameObject bullet;
    [HideInInspector]
    public bool died;

    AIPath astar;
    AIDestinationSetter destinationSetter;
    Rigidbody2D rb;
    SpriteRenderer sr;

    Animator anim;
    private static int gotHitAnimID;
    private static int isRunningAnimID;
    private static int isShootingAnimID;

    public float minRangeToShoot = 5f;

    LineRenderer lr;
    int playerCheckLayerMask;

    //Only used in turning (TurnLeft, TurnRight)
    private float sightsOriginalX;

    public float maxDistanceFromPlayer = 15f;

    AudioSource gunfireAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Components
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

        died = false;
        astar = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = GameManager.current.player;
        //First child should be the gameobject with the sprite and animator. 
        anim = transform.GetChild(0).GetComponent<Animator>();
        gotHitAnimID = Animator.StringToHash("GotHit");
        isRunningAnimID = Animator.StringToHash("IsRunning");
        isShootingAnimID = Animator.StringToHash("IsShooting");
        rb = GetComponent<Rigidbody2D>();
        lr = transform.GetChild(1).GetComponent<LineRenderer>();
        playerCheckLayerMask = LayerMask.GetMask("Obstacles", "Player");

        sightsOriginalX = lr.transform.localPosition.x;
        StartCoroutine(Act());

        gunfireAudio = GetComponent<AudioSource>();
    }

    public void TurnLeft()
    {
        sr.flipX = true;
        lr.transform.localPosition = new Vector3(-sightsOriginalX, lr.transform.localPosition.y, lr.transform.localPosition.z);
    }

    public void TurnRight()
    {
        sr.flipX = false;
        lr.transform.localPosition = new Vector3(sightsOriginalX, lr.transform.localPosition.y, lr.transform.localPosition.z);
    }
    IEnumerator Act()
    {
        
        while(true)
        {
            if (astar.velocity.x < 0)
            {
                //Flip sprite and move laser pointer to appropriate area
                TurnLeft();
            }
            else
            {
                TurnRight();
            }


            Vector3 playerPos = GameManager.current.player.position;
            Vector2 vectorToPlayer = playerPos - transform.position;
            RaycastHit2D ray = Physics2D.Linecast(transform.position, GameManager.current.player.position, playerCheckLayerMask);
            Debug.DrawLine(transform.position, playerPos, Color.gray, .3f);
            if (vectorToPlayer.sqrMagnitude < minRangeToShoot * minRangeToShoot && ray.collider != null &&
                ray.collider.transform == GameManager.current.player)
            {
                astar.canMove = false;
                GetComponent<Rigidbody2D>().isKinematic = true;
                anim.SetTrigger(isShootingAnimID);
                //SecondChild should be LineRenderer object for all hunters
                //Enabling the linerenderer should also cause the linehandler to activate and aim the linerenderer. 
                lr.enabled = true;
                yield return new WaitForSeconds(2f);
                lr.enabled = false;
                gunfireAudio.Play();
                Instantiate(bullet, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
                astar.canMove = true;
                GetComponent<Rigidbody2D>().isKinematic = true;
                anim.SetTrigger(isRunningAnimID);
                
                //Give player 3 seconds to breathe
                yield return new WaitForSeconds(3f);
            }
            if ((GameManager.current.player.position - transform.position).sqrMagnitude > maxDistanceFromPlayer * maxDistanceFromPlayer)
            {
                print("Destroyed Hunter");
                GameManager.current.removeEnemies(1);
                Destroy(gameObject);
            }
            //Delay before checking again
            yield return new WaitForSeconds(1f);

            
        }

        
        
    }

    public void GetShot()
    {
        if (!died)
        {
            StopAllCoroutines();
            died = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
            lr.enabled = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<CapsuleCollider2D>());
            astar.canSearch = false;
            astar.canMove = false;
            anim.SetTrigger(gotHitAnimID);
        }
        
    }
}
