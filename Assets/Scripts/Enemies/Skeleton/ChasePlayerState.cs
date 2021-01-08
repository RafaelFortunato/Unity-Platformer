using UnityEngine;

public class ChasePlayerStateModel
{
    public Transform playerTransform;
}

public class ChasePlayerState : IState
{
    private BaseAI baseAI;
    private ChasePlayerStateModel model;

    public ChasePlayerState(BaseAI baseAI, ChasePlayerStateModel model)
    {
        this.baseAI = baseAI;
        this.model = model;
    }

    public void Update()
    {
        int directionX = model.playerTransform.position.x > baseAI.transform.position.x ? 1 : -1;
        baseAI.transform.rotation = new Quaternion(0, directionX < 0 ? 180 : 0, 0, 0);

        var movePos = baseAI.transform.position + Vector3.right * (directionX * baseAI.walkSpeed * Time.deltaTime);
        baseAI.transform.position = movePos;
    }

    public void OnEnter()
    {
        baseAI.animator.SetBool("Walking", true);
    }

    public void OnExit()
    {
    }
}
