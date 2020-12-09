using UnityEngine;

public class BettlePatrolState : IState
{
    private BettleAI bettleAI;

    private float velocityX = 1;
    const float groundCheckRadius = .2f;

    public BettlePatrolState(BettleAI bettleAI)
    {
        this.bettleAI = bettleAI;
    }

    public void Update()
    {
        if (bettleAI.HasDelay() || !IsGrounded())
        {
            return;
        }

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
        Collider[] colliders = Physics.OverlapSphere(bettleAI.groundBelowCheck.position, groundCheckRadius, bettleAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != bettleAI.gameObject)
                return true;
        }

        return false;
    }

    private bool HasGroundAhead()
    {
        Collider[] colliders = Physics.OverlapSphere(bettleAI.groundAheadCheck.position, groundCheckRadius, bettleAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != bettleAI.gameObject)
                return true;
        }

        return false;
    }

    private void MoveFoward()
    {
        var movePos = bettleAI.transform.position + Vector3.right * (velocityX * bettleAI.walkSpeed * Time.deltaTime);
        bettleAI.transform.position = movePos;
        // bettleAI.rigidbody.MovePosition(movePos);
    }

    private void TurnAround()
    {
        velocityX = -velocityX;
        bettleAI.transform.rotation = new Quaternion(0, velocityX < 0 ? 180 : 0, 0, 0);
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
}