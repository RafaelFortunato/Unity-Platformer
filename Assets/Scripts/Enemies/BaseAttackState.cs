using UnityEngine;
using UnityEngine.Events;

public class AttackStateModel
{
    public Transform transform;
    public float range;
    public float delay;
    public int damage;
    public UnityEvent onAttackStrike;
}

public class BaseAttackState : IState
{
    private BaseAI baseAI;
    private AttackStateModel model;

    public BaseAttackState(BaseAI baseAI, AttackStateModel model)
    {
        this.baseAI = baseAI;
        this.model = model;
    }

    public void Update()
    {
        baseAI.controlDelay = model.delay;
        baseAI.animator.SetTrigger("Attack");
    }

    public void OnEnter()
    {
        model.onAttackStrike.AddListener(ApplyDamageToArea);
    }

    public void OnExit()
    {
        model.onAttackStrike.RemoveListener(ApplyDamageToArea);
    }

    private void ApplyDamageToArea()
    {
        Collider[] colliders = Physics.OverlapSphere(model.transform.position, model.range);
        Debug.Log("Colliders count " + colliders.Length);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                DamageController.ApplyDamage(collider.gameObject, model.damage);
            }
        }
    }
}
