using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVisable : MonoBehaviour {

    public Text p2join;
    private bool cycle = true;
    void Start()
    {
        StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
    }
    void Update ()
    {
        if (Input.GetButton("P2_Fire1"))
        {
            p2join.text = "";
            cycle = false;
        }
    }
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        if (cycle)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
            StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
        }
        else
        {
            GetComponent<Text>().color = new Color(0, 0, 0, 0);
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        if (cycle)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
        }
        else
        {
            GetComponent<Text>().color = new Color(0, 0, 0, 0);
        }
    }
}
