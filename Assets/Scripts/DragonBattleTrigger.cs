using UnityEngine;

public class DragonBattleTrigger : MonoBehaviour
{
    public GameObject dragonBattleBarrier;
    public DragonAI dragon;
    public PlayerController player;
    public AudioController audioController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            dragonBattleBarrier.SetActive(true);
            player.PausePlayer(3);
            dragon.WakeUp();
            audioController.FadeOutLevelMusic();
            gameObject.SetActive(false);
        }
    }
}
