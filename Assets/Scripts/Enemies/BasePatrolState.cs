using UnityEngine;

public class BasePatrolState : IState
{
    private BaseAI baseAI;

    private float directionX = 1;
    const float groundCheckRadius = .2f;

    public BasePatrolState(BaseAI baseAI)
    {
        this.baseAI = baseAI;
    }

    public void Update()
    {
        if (!IsGrounded())
        {
            return;
        }

        if (HasGroundAhead() && !HasObstacleAhead())
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
        Collider[] colliders = Physics.OverlapSphere(baseAI.groundBelowCheck.position, groundCheckRadius, baseAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != baseAI.gameObject)
                return true;
        }

        return false;
    }

    private bool HasGroundAhead()
    {
        Collider[] colliders = Physics.OverlapSphere(baseAI.groundAheadCheck.position, groundCheckRadius, baseAI.groundLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != baseAI.gameObject)
                return true;
        }

        return false;
    }

    private bool HasObstacleAhead()
    {
        Collider[] colliders = Physics.OverlapSphere(baseAI.obstacleAheadCheck.position, groundCheckRadius, baseAI.obstacleLayer);
        foreach (var c in colliders)
        {
            if (c.gameObject != baseAI.gameObject)
                return true;
        }

        return false;
    }

    private void MoveFoward()
    {
        var movePos = baseAI.transform.position + Vector3.right * (directionX * baseAI.walkSpeed * Time.deltaTime);
        baseAI.transform.position = movePos;
    }

    private void TurnAround()
    {
        directionX = -directionX;
        baseAI.transform.rotation = new Quaternion(0, directionX < 0 ? 180 : 0, 0, 0);
    }

    public void OnEnter()
    {
        baseAI.transform.rotation = new Quaternion(0, directionX < 0 ? 180 : 0, 0, 0);
        baseAI.animator.SetBool("Walking", true);
    }

    public void OnExit()
    {

    }
}