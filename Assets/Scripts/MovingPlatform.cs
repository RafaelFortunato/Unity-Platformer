using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Range(0, 1)]
    public float delta;

    [Min(0)]
    public float speed;

    public Rigidbody rigidbody;

    public Vector3 initialPosition;
    public Vector3 targetPosition;

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(initialPosition, targetPosition, Remap(Mathf.Cos(Time.time * speed), -1, 1, 0, 1));
    }

    private float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    private void OnValidate()
    {
        transform.position = Vector3.Lerp(initialPosition, targetPosition, delta);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER) || other.gameObject.CompareTag(Tags.ENEMY))
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER) || other.gameObject.CompareTag(Tags.ENEMY))
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
