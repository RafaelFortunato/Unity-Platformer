using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fireballSpeed;
    public int fireballDamage;

    void Start()
    {
        Invoke("Destroy", 6);
    }

    void FixedUpdate()
    {
        transform.position += transform.right * (fireballSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
           DamageController.ApplyDamage(other.gameObject, fireballDamage);
        }

        Destroy();
    }

    private void Destroy()
    {
        // TODO: Cool Fade?
        Destroy(gameObject);
    }
}
