using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    PlayerMovementScript player;
    float xSpeed;
    float enemyLiveCount=3 ;
    void Start()
    {
        player = FindObjectOfType<PlayerMovementScript>();
        myRigidbody = GetComponent<Rigidbody2D>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
    }
   
     void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
