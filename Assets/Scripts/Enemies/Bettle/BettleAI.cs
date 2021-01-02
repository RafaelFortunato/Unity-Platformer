public class BettleAI : BaseAI
{
    new void Start()
    {
        base.Start();

        var patrolState = new BasePatrolState(this);
        stateMachine.SetState(patrolState);
    }
}