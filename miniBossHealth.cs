using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniBossHealth : MonoBehaviour {

    public Image _bar;
    private float currentHealth;
    public int maxHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = DestroyByContact.mHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth != DestroyByContact.mHealth)
        {
            currentHealth = DestroyByContact.mHealth;
            HealthChange(currentHealth);
        }
    }

    void HealthChange(float currentHealth)
    {
        float amount = currentHealth / maxHealth;
        _bar.fillAmount = amount;
    }
}
