using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    LineRenderer lr;
    int layerMask;
    // Start is called before the first frame update
    HunterAI hunterBrain;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        layerMask = LayerMask.GetMask("Obstacles", "Player");
        hunterBrain = transform.parent.GetComponent<HunterAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lr.enabled)
        {
            if(GameManager.current.player.position.x < transform.parent.position.x)
            {
                hunterBrain.TurnLeft();
            } else
            {
                hunterBrain.TurnRight();
            }
            lr.SetPosition(0, transform.position);
            RaycastHit2D ray = Physics2D.Linecast(transform.position, GameManager.current.player.position, layerMask);
            if (ray.collider != null)
            {
                lr.SetPosition(1, ray.point);
            } else
            {
                lr.SetPosition(1, GameManager.current.player.position);
            }
        }
    }
}
