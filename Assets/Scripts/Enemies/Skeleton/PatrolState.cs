using System;
using UnityEngine;

public class PatrolState : IState
{
    private SkeletonAI skeletonAI;

    private float velocityX = 1;
    const float groundCheckRadius = .6f;

    public PatrolState(SkeletonAI skeletonAI)
    {
        this.skeletonAI = skeletonAI;
    }

    public void Update()
    {
        if (skeletonAI.HasDelay() || !IsGrounded())
        {
            return;
        }

        skeletonAI.animator.SetBool("Walking", true);

        if (HasGroundAhead())
        {
            MoveFoward();
        }
        else
        {
            TurnAround();
        }
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(skeletonAI.groundBelowCheck.position, groundCheckRadius, skeletonAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != skeletonAI.gameObject)
                return true;
        }

        return false;
    }

    private bool HasGroundAhead()
    {
        Collider[] colliders = Physics.OverlapSphere(skeletonAI.groundAheadCheck.position, groundCheckRadius, skeletonAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != skeletonAI.gameObject)
                return true;
        }

        return false;
    }

    private void MoveFoward()
    {
        var movePos = skeletonAI.transform.position + Vector3.right * (velocityX * skeletonAI.walkSpeed * Time.deltaTime);
        skeletonAI.transform.position = movePos;
        // bettleAI.rigidbody.MovePosition(movePos);
    }

    private void TurnAround()
    {
        velocityX = -velocityX;
        skeletonAI.transform.rotation = new Quaternion(0, velocityX < 0 ? 180 : 0, 0, 0);
    }

    public void OnEnter()
    {
        skeletonAI.transform.rotation = new Quaternion(0, velocityX < 0 ? 180 : 0, 0, 0);
    }

    public void OnExit()
    {

    }

    private void OnDrawGizmos()
    {
        if (skeletonAI.groundAheadCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(skeletonAI.groundAheadCheck.position, groundCheckRadius);

        // Handles.DrawWireDisc(SwordAttackPosition.position, Vector3.forward, SwordAttackRange);
    }
}
