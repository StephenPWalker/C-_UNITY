using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barLightOrder : MonoBehaviour {

    [SerializeField]
    private Image[] images;
    [SerializeField]
    private int currentCount;
    [SerializeField]
    private int newCount;
    private int maxCount;
    [SerializeField]
    private Color off;
    [SerializeField]
    private Color on;
    private bool changingColor;
	// Use this for initialization
	void Start ()
    {
        maxCount = images.Length;
        currentCount = Random.Range(0, maxCount);
        for (int i = 0; i < currentCount; i++)
        {
            images[i].color = on;
        }
        newCount = Random.Range(0, maxCount);
        ChangeColor();
        changingColor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (changingColor == false)
        {
            ChangeColor();
            changingColor = true;
        }
        if (newCount == currentCount)
        {
            newCount = Random.Range(0, maxCount);
            changingColor = false;
        }
    }
    void ChangeColor()
    {
        if (currentCount != newCount)
        {
            if (newCount < currentCount)
            {
                StartCoroutine(ChangeDown());
            }
            else if (newCount > currentCount)
            {
                StartCoroutine(ChangeUp());
            }
        }          
    }
    IEnumerator ChangeUp()
    {
        for (int i = currentCount; i <= newCount; i++)
        {
            yield return new WaitForSeconds(0.25f);
            images[i].color = on;
            currentCount = i;
        }
    }
    IEnumerator ChangeDown()
    {
        for (int i = currentCount; i >= newCount; i--)
        {
            yield return new WaitForSeconds(0.25f);
            images[i].color = off;
            currentCount = i;
        }
    }
}
