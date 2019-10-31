using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldView : MonoBehaviour {

    public string myTag;
    public GameObject shield;
    // Update is called once per frame
    void Update()
    {
        string tag = transform.root.tag;
        myTag = tag;
        if (tag == "Player")
        {
            if (GameController.playerShield > 0 && PlayerController.isShieldOn)
            {
                shield.SetActive(true);
            }

            if (GameController.playerShield <= 0 || PlayerController.isShieldOn == false)
            {
                shield.SetActive(false);
            }
        }
        if (tag == "Player2")
        {
            if (GameController.player2Shield > 0 && PlayerController.isShield2On)
            {
                shield.SetActive(true);
            }

            if (GameController.player2Shield <= 0 || PlayerController.isShield2On == false)
            {
                shield.SetActive(false);
            }
        }
    }
}
