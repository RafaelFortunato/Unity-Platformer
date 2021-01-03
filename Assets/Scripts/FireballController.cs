using UnityEngine;

public class FireballModel
{
    public string ownerTag;
    public string hitTag;
    public Vector3 originPosition;
    public Vector3 direction;
    public int damage;
    public float speed;
}

public static class FireballController
{
    private const string FIREBALL_PREFAB_PATH = "Prefabs/Fireball";
    private static GameObject fireballPrefab;

    public static void CreateFireball(FireballModel model)
    {
        if (fireballPrefab == null)
        {
            fireballPrefab = Resources.Load<GameObject>(FIREBALL_PREFAB_PATH);
        }

        var fireballObject = Object.Instantiate(fireballPrefab, model.originPosition, Quaternion.LookRotation(model.direction, Vector3.up));
        var fireballCollision = fireballObject.GetComponent<Fireball>();
        fireballCollision.direction = model.direction;
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