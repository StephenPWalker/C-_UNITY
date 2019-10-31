using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visability : MonoBehaviour {

    public Text t;
    public string whichPlayer;
	// Use this for initialization
	void Start ()
    {
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (whichPlayer == "Player1")
        {
            if (CharacterSelect.stateGlobal == "Selected")
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
            }
            else
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
            }
        }

        if (whichPlayer == "Player2")
        {
            if (CharacterSelect.stateGlobal2 == "Selected")
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
            }
            else
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
            }
        }
    }
}
