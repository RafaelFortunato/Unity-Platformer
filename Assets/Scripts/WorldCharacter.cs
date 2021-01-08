using System.Collections;
using UnityEngine;

public class WorldCharacter : MonoBehaviour
{
    private SkinnedMeshRenderer[] meshRenderers;
    private MaterialPropertyBlock materialPropertyBlock;
    public AudioSource damageTakenSound;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_Color", Color.red);
    }

    public void DamageTakenAnimation(float duration)
    {
        StartCoroutine(DamageTakenCoroutine(duration));
        damageTakenSound.Play();
    }

    public IEnumerator DamageTakenCoroutine(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            foreach (var mesh in meshRenderers)
            {
                mesh.SetPropertyBlock(materialPropertyBlock);
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var mesh in meshRenderers)
            {
                mesh.SetPropertyBlock(null);
            }

            yield return new WaitForSeconds(0.1f);

            duration -= Time.deltaTime;
        }

        foreach (var mesh in meshRenderers)
        {
            mesh.SetPropertyBlock(null);
        }
    }
}