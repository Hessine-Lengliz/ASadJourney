using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidbody;
    float liveCount;
    
    
    
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        liveCount = 3;
    }


    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }
    void ontriggerenter2D(Collider2D collision)
    {
        if(collision.tag == "Arrow")
        {
            liveCount--;
            Debug.Log(liveCount);
        }
    }
    
       
    
    void OnTriggerExit2D(Collider2D other)
    {
        
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
       
        

    }
     void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody.velocity.x)), 1f);

    }

}