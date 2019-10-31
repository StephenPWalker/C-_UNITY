using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

    private int health;
    private bool restart = true;
    private bool restart2 = true;
    public string who;
    void Start()
    {
        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0);
    }
    void Update()
    {
        if (who == "Player1")
        {
            health = GameController.playerHealth;
            if (health <= 0)
            {
                if (restart)
                {
                    restart = false;
                    StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
                }
            }
        }
        if (who == "Player2")
        {
            health = GameController.player2Health;
            if (health <= 0)
            {
                if (restart2)
                {
                    restart2 = false;
                    StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
                }
            }
        }

        if (health > 0)
        {
            GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0);
            if (who == "Player1")
            {
                restart = true;
            }
            if (who == "Player2")
            {
                restart2 = true;
            }
        }

    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
    }
}
