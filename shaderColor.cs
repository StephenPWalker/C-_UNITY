using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderColor : MonoBehaviour {

    [SerializeField]
    private Color p1color;
    [SerializeField]
    private Color p2color;
    [SerializeField]
    private Color enemy;
    [SerializeField]
    private Color ally1;
    [SerializeField]
    private Color ally2;
    [SerializeField]
    private Color ally3;
    // Use this for initialization
    void Start()
    {
        gameObject.layer = 0;
        if (transform.root.tag == "Player")
        {
            GetComponent<Renderer>().material.color = p1color;
        }
        else if (transform.root.tag == "Player2")
        {
            GetComponent<Renderer>().material.color = p2color;
        }
        else if (transform.root.tag == "Enemy")
        {
            GetComponent<Renderer>().material.color = enemy;
        }
        else if (transform.root.tag == "Ally")
        {
            int which = Random.Range(0, 99);
            if (which < 100)
            {
                GetComponent<Renderer>().material.color = ally1;
            }
            else if (which < 64)
            {
                GetComponent<Renderer>().material.color = ally2;
            }
            else if (which < 34)
            {
                GetComponent<Renderer>().material.color = ally3;
            }
        }
    }
}
