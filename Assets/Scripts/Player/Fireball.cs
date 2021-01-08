using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public FireballModel model;
    private const float speed = 20f;

    private const float lifetime = 6f;

    void Start()
    {
        Invoke("Destroy", lifetime);
    }

    void Update()
    {
        transform.position += model.direction * (Time.deltaTime * speed);
    }

    void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Fireball OnCollisionEnter: " + other.gameObject.tag);
        FireballController.FireballCollision(this, other);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
