using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

    public Image _bar;

	// Update is called once per frame
	void Update ()
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
    }
}
