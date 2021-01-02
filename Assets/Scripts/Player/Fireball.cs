using UnityEngine;

public class Fireball : MonoBehaviour
{
    public FireballModel model;

    private const float lifetime = 6f;

    void Start()
    {
        Invoke("Destroy", lifetime);
    }

    private void OnCollisionEnter(Collision other)
    {
        FireballController.FireballCollision(this, other);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
