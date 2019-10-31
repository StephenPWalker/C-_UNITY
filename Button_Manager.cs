using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class Button_Manager : MonoBehaviour {

    AsyncOperation ao;
    private string levelName;
    public static int players;
    private string state;
    public Image[] buttons;
    private Color baseColor;
    public Color highlight;

    
    public void Start()
    {
        //baseColor = buttons[0].GetComponent<Image>().color;
        //state = "null";
        ao = SceneManager.LoadSceneAsync(2);
        ao.allowSceneActivation = false;
    }
    /*
    public void Update()
    {
        if (Input.GetAxis("Vertical") != 0 && state == "null")
        {
            state = "1Player";
            buttons[0].GetComponent<Image>().color = highlight;
        }
        if (state == "1Player" && Input.GetButtonUp("Submit"))
        {
            NewGameButton("Char_Select");
        }
        if (Input.GetAxis("Vertical") < 0 && state == "1Player")
        {
            buttons[0].GetComponent<Image>().color = baseColor;
            state = "2Player";
            buttons[1].GetComponent<Image>().color = highlight;
        }
    }
    */

    public void NewGameButton(string newGameLevel)  
    {
        players = 1;
        ao.allowSceneActivation = true;
        SceneManager.LoadScene(newGameLevel);
    }

    public void NewGameButton2(string newGameLevel)
    {
        players = 2;
        ao.allowSceneActivation = true;
        SceneManager.LoadScene(newGameLevel);           
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
