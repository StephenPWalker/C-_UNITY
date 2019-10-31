using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour {

    public Image _bar;
    private float currentHealth;
    public float FadeRate;
    float amount;

    // Use this for initialization
    void Start ()
    {
        if (transform.root.tag == "Player")
        {
            currentHealth = GameController.playerHealth;
        }
        if (transform.root.tag == "Player2")
        {
            currentHealth = GameController.player2Health;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

        _bar = GetComponent<Image>();
        Color c = _bar.color;
        if (transform.root.tag == "Player")
        {
            if (currentHealth != GameController.playerHealth)
            {
                c.a = 1.0f;
                _bar.color = c;
                currentHealth = GameController.playerHealth;
                HealthChange(currentHealth);
            }
            else
            {
                c.a = Mathf.Lerp(c.a, 0.0f, FadeRate * Time.deltaTime);
                _bar.color = c;
            }
        }
        if (transform.root.tag == "Player2")
        {
            if (currentHealth != GameController.player2Health)
            {
                c.a = 1.0f;
                _bar.color = c;
                currentHealth = GameController.player2Health;
                HealthChange(currentHealth);
            }
            else
            {
                c.a = Mathf.Lerp(c.a, 0.0f, FadeRate * Time.deltaTime);
                _bar.color = c;
            }
        }       
	}

    void HealthChange(float currentHealth)
    {
        if (transform.root.tag == "Player")
        {
            amount = currentHealth / GameController.playerHealthMax;
        }
        if (transform.root.tag == "Player2")
        {
            amount = currentHealth / GameController.player2HealthMax;
        }
            _bar.fillAmount = amount;
    }
}
