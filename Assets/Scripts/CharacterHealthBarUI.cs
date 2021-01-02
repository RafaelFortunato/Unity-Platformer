using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthBarUI : MonoBehaviour
{
    public Health healthModel;
    public GameObject lifeBarPrefab;

    private List<Image> lifeBars = new List<Image>();
    private int currentHealth;

    void Start()
    {
        for (int i = 0; i < healthModel.MaxHealth; i++)
        {
            var lifeBarObj = Instantiate(lifeBarPrefab, transform);
            lifeBars.Add(lifeBarObj.GetComponent<Image>());
        }

        currentHealth = healthModel.MaxHealth;
        healthModel.OnHealthChanged += UpdateHealthBar;
    }

    private void UpdateHealthBar(int health)
    {
        health = Math.Max(0, health);

        for (; currentHealth < health; currentHealth++)
        {
            lifeBars[currentHealth].color = Color.red;
        }

        for (; currentHealth > health; currentHealth--)
        {
            lifeBars[currentHealth - 1].color = new Color(0.2f, 0.2f, 0.2f);
        }
    }
}
