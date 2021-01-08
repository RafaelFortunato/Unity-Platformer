using UnityEngine;

public class DragonAI: BaseAI
{
    [HideInInspector] public Transform playerTransform;
    public Transform flameOriginPosition;
    public Transform flyPositionLeft;
    public Transform flyPositionRight;
    public Health health;
    public GameUI gameUI;
    public GameObject fireballPrefab;

    [Header("Sounds")]
    public AudioSource fireballCreation;
    public AudioSource dragonRoar;

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
            prefab = fireballPrefab,
            ownerTag = Tags.ENEMY,
            hitTag = Tags.PLAYER,
            originPosition = flameOriginPosition.position,
            direction = fireballDirection,
            damage = 1
        };

        FireballController.CreateFireball(fireballModel);
        fireballCreation.PlayOneShot(fireballCreation.clip, 0.75f);
    }

    public void WakeUp()
    {
        animator.SetBool("Awake", true);
        Invoke("StartDragonBattle", 3);
        dragonRoar.Play();
    }

    public void StartDragonBattle()
    {
        health.enabled = true;
        SetupMovementBehaviour();
        audioController.PlayDragonBattleMusic();
    }

    public override void DeathAnimation()
    {
        controlDelay = 3;
        animator.SetTrigger("Dead");
        dragonRoar.pitch = 1.5f;
        dragonRoar.Play();
        audioController.LowerBattleMusic(1.2f);
    }

    public void ShowVictoryPanel()
    {
        gameUI.ShowVictoryPanel();
    }
}