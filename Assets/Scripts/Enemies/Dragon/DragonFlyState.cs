using System.Collections.Generic;
using UnityEngine;

public class DragonFlyStateModel
{
    public Vector3 flyPositionLeft;
    public Vector3 flyPositionRight;
    public Vector3 landPosition;
}

public class DragonFlyStateAnimationModel
{
    public Vector3 beginPosition;
    public Vector3 endPosition;
    public float beginRotation;
    public float endRotation;
    public float duration;
    public bool isAttacking;
    public bool isFlying;
}

public class DragonFlyState : IState
{
    private DragonAI baseAI;
    private DragonFlyStateModel model;

    private const float flyTranslateDuration = 3;
    private const float horizontalGlideDuration = 3;

    private float time;
    private Queue<DragonFlyStateAnimationModel> flyAnimations;
    private DragonFlyStateAnimationModel currentAnimation;

    public DragonFlyState(DragonAI baseAI, DragonFlyStateModel model)
    {
        this.baseAI = baseAI;
        this.model = model;
    }

    public void Update()
    {
        time += Time.deltaTime;
        var delta = time / currentAnimation.duration;
        if (delta > 1)
        {
            if (flyAnimations.Count == 0)
            {
                baseAI.Land();
                return;
            }

            NextAnimation();
            delta = time / currentAnimation.duration;
        }

        UpdateAnimation(delta);
    }

    private void NextAnimation()
    {
        time = 0;
        currentAnimation = flyAnimations.Dequeue();
        baseAI.animator.SetBool("Flying", currentAnimation.isFlying);
        baseAI.animator.SetBool("Attacking", currentAnimation.isAttacking);
    }

    private void UpdateAnimation(float delta)
    {
        baseAI.transform.position = Vector3.Lerp(currentAnimation.beginPosition, currentAnimation.endPosition, delta);
        baseAI.transform.rotation = Quaternion.Euler(0, Mathf.Lerp(currentAnimation.beginRotation, currentAnimation.endRotation, delta), 0);
    }

    public void OnEnter()
    {
        flyAnimations = new Queue<DragonFlyStateAnimationModel>();

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.landPosition,
            endPosition = model.flyPositionRight,
            beginRotation = 270,
            endRotation = 180,
            duration = flyTranslateDuration,
            isFlying = true,
            isAttacking = false
        });

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.flyPositionRight,
            endPosition = model.flyPositionLeft,
            beginRotation = 180,
            endRotation = 180,
            duration = horizontalGlideDuration,
            isFlying = true,
            isAttacking = true
        });

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.flyPositionLeft,
            endPosition = model.flyPositionRight,
            beginRotation = 180,
            endRotation = 180,
            duration = horizontalGlideDuration,
            isFlying = true,
            isAttacking = true
        });

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.flyPositionRight,
            endPosition = model.flyPositionLeft,
            beginRotation = 180,
            endRotation = 180,
            duration = horizontalGlideDuration,
            isFlying = true,
            isAttacking = true
        });

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.flyPositionLeft,
            endPosition = model.flyPositionRight,
            beginRotation = 180,
            endRotation = 180,
            duration = horizontalGlideDuration,
            isFlying = true,
            isAttacking = true
        });

        flyAnimations.Enqueue(new DragonFlyStateAnimationModel
        {
            beginPosition = model.flyPositionRight,
            endPosition = model.landPosition,
            beginRotation = 180,
            endRotation = 270,
            duration = flyTranslateDuration,
            isFlying = false,
            isAttacking = false
        });

        NextAnimation();
    }

    public void OnExit()
    {
    }
}