using System.Collections;
using UnityEngine;

public class WorldCharacter : MonoBehaviour
{
    private SkinnedMeshRenderer[] meshRenderers;
    private MaterialPropertyBlock materialPropertyBlock;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
        materialPropertyBlock.SetColor("_Color", Color.red);
    }

    public void DamageTakenAnimation(float duration)
    {
        StartCoroutine(DamageTakenCoroutine(duration));
    }

    public IEnumerator DamageTakenCoroutine(float duration)
    {
        foreach (var mesh in meshRenderers)
        {
            mesh.SetPropertyBlock(materialPropertyBlock);
        }

        yield return new WaitForSeconds(duration);

        foreach (var mesh in meshRenderers)
        {
            mesh.SetPropertyBlock(null);
        }
    }
}