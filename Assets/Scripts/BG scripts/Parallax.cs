using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject Cameras;
    public float parallaxEffect;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }


    void FixedUpdate()
    {
        float dist = (Cameras.transform.position.x * parallaxEffect);
        transform.position =  new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
