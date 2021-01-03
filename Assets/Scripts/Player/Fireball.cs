using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public FireballModel model;
    [HideInInspector] public Vector3 direction;
    private const float speed = 20f;

    private const float lifetime = 6f;

    void Start()
    {
        Invoke("Destroy", lifetime);
    }

    void Update()
    {
        transform.position += direction * (Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter: " + other.gameObject.tag);
        FireballController.FireballCollision(this, other);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
