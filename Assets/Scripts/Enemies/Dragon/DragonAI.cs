using UnityEngine;

public class DragonAI: BaseAI
{
    [HideInInspector] public Transform playerTransform;
    public Transform flameOriginPosition;
    public Transform flyPositionLeft;
    public Transform flyPositionRight;
    public Health health;
    public GameUI gameUI;

    private const float flameSpeed = 1000;

    private DragonFlyState flyState;
    private DragonLandState landState;

    new void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void SetupMovementBehaviour()
    {
        var flyStateModel = new DragonFlyStateModel
        {
            flyPositionLeft = flyPositionLeft.position,
            flyPositionRight = flyPositionRight.position,
            landPosition = transform.position
        };

        flyState = new DragonFlyState(this, flyStateModel);

        var landStateModel = new DragonLandStateModel();
        landState = new DragonLandState(this, landStateModel);
        stateMachine.SetState(landState);
    }

    public void Fly()
    {
        stateMachine.SetState(flyState);
    }

    public void Land()
    {
        stateMachine.SetState(landState);
    }

    public void CreateFireball()
    {
        var fireballDirection = (playerTransform.position - flameOriginPosition.position).normalized;

        var fireballModel = new FireballModel
        {
            ownerTag = Tags.ENEMY,
            hitTag = Tags.PLAYER,
            originPosition = flameOriginPosition.position,
            direction = fireballDirection,
            damage = 1,
            speed = flameSpeed
        };

        FireballController.CreateFireball(fireballModel);
    }

    public void WakeUp()
    {
        animator.SetBool("Awake", true);
        Invoke("EnableHealthComponent", 3);
    }

    public void EnableHealthComponent()
    {
        health.enabled = true;
        SetupMovementBehaviour();
    }

    public override void DeathAnimation()
    {
        controlDelay = 3;
        animator.SetTrigger("Dead");
    }

    public void ShowVictoryPanel()
    {
        gameUI.ShowVictoryPanel();
    }
}