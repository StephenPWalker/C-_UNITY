using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  myTag : MonoBehaviour {

    private string tag;
    [SerializeField]
    private string tag1;
    [SerializeField]
    private string tag2;
	// Use this for initialization
	void Start ()
    {
        tag = transform.root.tag;
        if (tag == "Player")
        {
            transform.gameObject.tag = tag1;
        }
        else if (tag == "Player2")
        {
            transform.gameObject.tag = tag2;
        }
	}
}
