using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStart : MonoBehaviour
{
    [SerializeField] private AudioClip hyperLevel1;

    void Awake()
    {
       
    }
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            SoundManager.instance.PlaySound(hyperLevel1);
            
        }
        

    }
   
}
 