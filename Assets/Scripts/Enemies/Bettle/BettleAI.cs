public class BettleAI : BaseAI
{
    void Start()
    {
        base.Start();

        var patrolState = new BasePatrolState(this);
        stateMachine.SetState(patrolState);
    }
}