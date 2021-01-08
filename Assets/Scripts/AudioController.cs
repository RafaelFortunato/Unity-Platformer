using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioSource dragonBattleMusic;
    public AudioSource enemyExplosion;
    public AudioSource itemCollected;

    public void PlayEnemyExplosion()
    {
        enemyExplosion.Play();
    }

    public void PlayItemCollected()
    {
        itemCollected.Play();
    }

    public void FadeOutLevelMusic()
    {
        StartCoroutine("FadeOutLevelMusicCoroutine");
    }

    private IEnumerator FadeOutLevelMusicCoroutine()
    {
        float delta = 2;
        while (delta > 0)
        {
            delta -= Time.deltaTime;
            levelMusic.volume = delta / 2;
            yield return null;
        }

        yield return null;
    }

    public void PlayDragonBattleMusic()
    {
        dragonBattleMusic.Play();
    }

    public void LowerBattleMusic(float pitch)
    {
        if (dragonBattleMusic.isPlaying)
        {
            dragonBattleMusic.volume = 0.35f;
            dragonBattleMusic.pitch = pitch;
        }
    }
}
