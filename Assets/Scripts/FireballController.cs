using UnityEngine;

public class FireballModel
{
    public GameObject prefab;
    public string ownerTag;
    public string hitTag;
    public Vector3 originPosition;
    public Vector3 direction;
    public int damage;
}

public static class FireballController
{

    public static void CreateFireball(FireballModel model)
    {
        var fireballObject = Object.Instantiate(model.prefab, model.originPosition, Quaternion.LookRotation(model.direction, Vector3.up));
        var fireballCollision = fireballObject.GetComponent<Fireball>();
        fireballCollision.model = model;
    }

    public static void FireballCollision(Fireball fireball, Collision other)
    {
        if (other.gameObject.CompareTag(fireball.model.ownerTag))
        {
            return;
        }

        if (other.gameObject.CompareTag(fireball.model.hitTag))
        {
            DamageController.ApplyDamage(other.gameObject, fireball.model.damage);
        }

        Object.Destroy(fireball.gameObject);
    }
}