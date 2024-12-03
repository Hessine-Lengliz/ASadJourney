using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rbB;
    public float fallDelay = 10f ;
   void Start()
    {
        rbB = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
     if(col.collider.CompareTag("Player"))
        {
            StartCoroutine(Fall());

        }
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rbB.isKinematic = false;
        
        yield return 0;
    }
}
