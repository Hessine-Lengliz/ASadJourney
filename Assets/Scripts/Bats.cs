using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class Bats : MonoBehaviour
{
    public AIPath aiPath;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if (aiPath != null)
        {
            myAnimator.SetBool("isFlying", true);
        
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f,1f, 1f);
        }
        else if(aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        }
        else
        {

            myAnimator.SetBool("isFlying", false);
        }
    }
     void OnTriggerExit2D(Collider2D collision)
    {
        
        FlipBatFacing();
    }

     void FlipBatFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }

}
