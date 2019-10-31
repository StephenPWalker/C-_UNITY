using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1p2hue : MonoBehaviour {


    // Update is called once per frame
    private void Start()
    {
        if (transform.root.tag == "Player")
        {
            GetComponent<ParticleSystem>().startColor = new Color(255, 0, 0, .25f);
        }
        else
        {
            GetComponent<ParticleSystem>().startColor = new Color(0, 0, 255, .25f);
        }
    }
}
