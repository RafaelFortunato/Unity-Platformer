using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerState : IState
{
    private SkeletonAI skeletonAI;

    public ChasePlayerState(SkeletonAI skeletonAI)
    {
        this.skeletonAI = skeletonAI;
    }

    public void Update()
    {
        if (skeletonAI.HasDelay())
        {
            return;
        }

        int directionX = skeletonAI.playerTransform.position.x > skeletonAI.transform.position.x ? 1 : -1;
        if (directionX < 0)
        {
            skeletonAI.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            skeletonAI.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        var movePos = skeletonAI.transform.position + Vector3.right * (directionX * skeletonAI.walkSpeed * Time.deltaTime);
        // skeletonAI.rigidbody.MovePosition(movePos);
        skeletonAI.transform.position = movePos;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}
