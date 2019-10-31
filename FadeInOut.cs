using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField]
    private Image image;
    [SerializeField]
    private float FadeTime;
    [SerializeField]
    private AudioClip sfx;
    AudioSource source;
    [SerializeField]
    private float elapsedTime = 0;
    private float volLow = 0.2f;
    private float volHigh = 0.5f;
    private float volume;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        source = GetComponent<AudioSource>();
        c = image.color;
        c.a = 0.0f;
        image.color = c;
    }
    private void Update()
    {
        if (isActive)
        {
            StartCoroutine(fadeInFadeOut());
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(sfx, volume);
            isActive = false;
        }
    }
    public void setIsActive()
    {
        isActive = true;
    }
    public bool getIsActive()
    {
        return isActive;
    }
    IEnumerator fadeInFadeOut()
    {
        for (int i = 0; i < 3; i++)
        {
            while (elapsedTime < FadeTime)
            {
                c = image.color;
                elapsedTime += Time.deltaTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, elapsedTime / FadeTime);
                yield return new WaitForSeconds(0.01f);
            }
            while (elapsedTime > 0)
            {
                c = image.color;
                elapsedTime -= Time.deltaTime;
                image.color = new Color(image.color.r, image.color.g, image.color.b, elapsedTime / FadeTime);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
