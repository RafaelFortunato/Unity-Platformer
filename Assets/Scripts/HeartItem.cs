using System;
using UnityEngine;

public class HeartItem : MonoBehaviour
{
    public int recoveryAmount;
    public AudioController audioController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER))
        {
            var healthComponent = other.gameObject.GetComponent<Health>();
            if (healthComponent == null)
                return;

            healthComponent.CurrentHealth = Math.Min(healthComponent.CurrentHealth + recoveryAmount, healthComponent.MaxHealth);

            audioController.PlayItemCollected();
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        transform.position += Vector3.up * (Mathf.Cos(Time.time) * 0.003f);
    }
}
