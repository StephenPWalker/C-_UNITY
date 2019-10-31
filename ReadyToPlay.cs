using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToPlay : MonoBehaviour {

    public Image i;
    public Text t;
    // Use this for initialization
    void Start()
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
            if (CharacterSelect.stateGlobal == "Selected" && CharacterSelect.stateGlobal2 == "Selected")
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, 0.875f);
                t.color = new Color(t.color.r, t.color.g, t.color.b, 1);
            }
            else
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
            }       
    }
}
