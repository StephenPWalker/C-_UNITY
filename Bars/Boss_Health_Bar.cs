using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Health_Bar : MonoBehaviour
{

    public Image _bar;
    private float currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = GameController.bossHealth;
    }

    // Update is called once per frame
    void Update()
    {      
        _bar = GetComponent<Image>();
        Color c = _bar.color;
        if (GameController.bossOn == false)
        {
            c.a = 0.0f;
            _bar.color = c;
        }
        else
        {
            c.a = 1.0f;
            _bar.color = c;
        }

        if (currentHealth < 25)
        {
            c.r = 1;
            c.g = 1;
            c.b = 1;
        }
        else if (currentHealth >= 25 && currentHealth < 50)
        {
            c.r = 1;
            c.g = 1;
            c.b = 0;
        }
        else if (currentHealth >= 50 && currentHealth < 75)
        {
            c.r = 0.1f;
            c.g = 1;
            c.b = 0;
        }
        else if (currentHealth >= 75)
        {
            c.r = 0;
            c.g = 1;
            c.b = 0;
        }
        if (currentHealth != GameController.bossHealth)
        {
            currentHealth = GameController.bossHealth;
            HealthChange(currentHealth);
        }
    }

    void HealthChange(float currentHealth)
    {
        float amount = currentHealth / GameController.bossHealthMax;
        _bar.fillAmount = amount;
    }
}
