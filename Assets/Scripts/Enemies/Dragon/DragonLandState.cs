using System.Collections.Generic;
using UnityEngine;

public class DragonLandStateModel
{

}

public class DragonLandStateAnimationModel
{
    public float duration;
    public bool isAttacking;

}

public class DragonLandState : IState
{
    private DragonAI baseAI;
    private DragonLandStateModel model;

    private const float idleDuration = 2;
    private const float attackDuration = 3;

    private float duration;
    private Queue<DragonLandStateAnimationModel> landAnimations;
    private DragonLandStateAnimationModel currentAnimation;

    public DragonLandState(DragonAI baseAI, DragonLandStateModel model)
    {
        this.baseAI = baseAI;
        this.model = model;
    }

    public void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            if (landAnimations.Count == 0)
            {
                baseAI.Fly();
                return;
            }

            NextAnimation();
        }
    }

    private void NextAnimation()
    {
        currentAnimation = landAnimations.Dequeue();
        baseAI.animator.SetBool("Attacking", currentAnimation.isAttacking);
        duration = currentAnimation.duration;
    }

    public void OnEnter()
    {
        landAnimations = new Queue<DragonLandStateAnimationModel>();

        landAnimations.Enqueue(new DragonLandStateAnimationModel
        {
            duration = attackDuration,
            isAttacking = true
        });

        landAnimations.Enqueue(new DragonLandStateAnimationModel
        {
            duration = idleDuration,
            isAttacking = false
        });

        landAnimations.Enqueue(new DragonLandStateAnimationModel
        {
            duration = attackDuration,
            isAttacking = true
        });

        landAnimations.Enqueue(new DragonLandStateAnimationModel
        {
            duration = idleDuration,
            isAttacking = false
        });
    }

    public void OnExit()
    {

    }
}