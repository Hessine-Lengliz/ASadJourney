using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 2f;
    [SerializeField] private float shakeMagnitude =10f;
    [SerializeField] private float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private float shakeTimer;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeTimer -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void Shake()
    {
        Debug.Log("Shaking!");
        shakeTimer = shakeDuration;
    }
}
