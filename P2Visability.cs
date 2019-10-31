using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P2Visability : MonoBehaviour {

    public GameObject parent;

    // Use this for initialization
    void Start()
    {
        if (Button_Manager.players == 2)
        {
            parent.SetActive(true);
        }
        else
        {
            parent.SetActive(false);
        }
    }
}
