using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerController player;

    public void AttackAction()
    {
        player.AttackAction();
    }

    public void FireballAction()
    {
        player.FireballAction();
    }
}
