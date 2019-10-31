using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBarFill : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private Image bar;
    [SerializeField]
    private float fillSpeed;
    [SerializeField]
    private float curValue;
    [SerializeField]
    private float newValue;
    [SerializeField]
    private float fillAmount;

    private void Start()
    {
        fillSpeed = Random.Range(1, 10);
        bar = GetComponent<Image>();
        curValue = Random.Range(0, 100) / 100f;
        bar.fillAmount = curValue;
        newValue = Random.Range(0, 100) / 100f;
        StartCoroutine(AmountChange());
    }
    private void Update()
    {
        fillAmount = bar.fillAmount;
        lerpValue();
    }
    private void lerpValue()
    {
        if (newValue != bar.fillAmount)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, newValue, Time.deltaTime * fillSpeed);
        }
    }
    IEnumerator AmountChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(fillSpeed);
            curValue = newValue;
            newValue = Random.Range(0, 100) / 100f;
        }
    }
}
