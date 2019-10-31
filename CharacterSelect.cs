using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    AsyncOperation ao;
    //audio
    public AudioClip Viper;
    public AudioClip Hornet;
    public AudioClip Scorpion;
    private AudioSource source;
    public AudioClip Move;
    private float volLow = 0.25f;
    private float volHigh = 0.75f;
    private float volume;
    //ship info
    public Text Info;
    public Text Info2;
    public Text p2Info;
    public Text p2Info2;
    public Text shipName1;
    public Text shipName2;
    public Sprite[] ships;
    public Image p1ShipImage;
    public Image p2ShipImage;
    //player 1
    public Image[] p1Bars;
    private string ship;
    private string shipType;
    private float health;
    private float speed;
    private float size;
    private float power;
    private string weapon1;
    private string weapon2;
    private string weapon3;
    private string weapon4;
    private string state;
    //player 2
    public Image[] p2Bars;
    private string p2ship;
    private string p2shipType;
    private float p2health;
    private float p2speed;
    private float p2size;
    private float p2power;
    private string p2weapon1;
    private string p2weapon2;
    private string p2weapon3;
    private string p2weapon4;
    private string p2state;
    //others
    private bool confirm = false;
    private bool waitingToConfirm = true;
    private bool pick = true;
    private bool p2pick = true;
    public static string stateGlobal;
    public static string stateGlobal2;
    private int players;
    public static int howManyPlayers;
    // Bar values
    private float healthBarMax = 125;
    private float speedBarMax = 120;
    private float sizeBarMax = 100;
    private float powerBarMax = 100;
    private float overallBarMax = 445;
    private bool isPlayingP1Audio = false;
    private bool isPlayingP2Audio = false;
    private bool nowPlayingAudio = false;
    void Start()
    {

        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;
        source = GetComponent<AudioSource>();

        players = Button_Manager.players;

        ship = "Hornet";
        state = ship;
        shipType = "Defense fighter";
        health = 100;
        speed = 8;
        size = 50;
        power = 150; // use for ability
        weapon1 = "Dual Machine Gun";
        weapon2 = "Spread Shot";
        weapon3 = "Drones";
        weapon4 = "Super Spread";

        p2ship = "Hornet";
        p2state = ship;
        p2shipType = "Defense fighter";
        p2health = 100;
        p2speed = 8;
        p2size = 50;
        p2power = 150; // use for ability
        p2weapon1 = "Dual Machine Gun";
        p2weapon2 = "Spread Shot";
        p2weapon3 = "Drones";
        p2weapon4 = "Super Spread";

        SetText("P1");
        SetText("P2");
        p1ShipImage.sprite = ships[0];
        if (players == 2)
        {
            p2ShipImage.sprite = ships[0];
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        stateGlobal = state;
        stateGlobal2 = p2state;
        howManyPlayers = players;
        if (players == 2 && state == "Selected" && p2state == "Selected")
        {
            StartCoroutine(LoadWait());   
        }
        else if (players == 1 && state == "Selected")
        {
            StartCoroutine(LoadWait());
        }

        //player 1 movement
        if ((Input.GetAxis("P1_Horizontal") > 0.5) && state == "Hornet" && pick)
        {
            pick = false;
            Ship2("P1");
            MoveAudio();
        }
        if ((Input.GetAxis("P1_Horizontal") < -0.5) && state == "Hornet" && pick)
        {
            pick = false;
            Ship3("P1");
            MoveAudio();
        }
        if ((Input.GetAxis("P1_Horizontal") > 0.5) && state == "Scorpion" && pick)
        {
            pick = false;
            Ship3("P1");
            MoveAudio();
        }
        if ((Input.GetAxis("P1_Horizontal") < -0.5) && state == "Scorpion" && pick)
        {
            pick = false;
            Ship1("P1");
            MoveAudio();
        }
        if ((Input.GetAxis("P1_Horizontal") > 0.5) && state == "Viper" && pick)
        {
            pick = false;
            Ship1("P1");
            MoveAudio();
        }
        if ((Input.GetAxis("P1_Horizontal") < -0.5) && state == "Viper" && pick)
        {
            pick = false;
            Ship2("P1");
            MoveAudio();
        }

        if (players == 2)
        {
            //player 2 movement
            if ((Input.GetAxis("P2_Horizontal") > 0.5) && p2state == "Hornet" && p2pick)
            {
                p2pick = false;
                Ship2("P2");
                MoveAudio();
            }
            if ((Input.GetAxis("P2_Horizontal") < -0.5) && p2state == "Hornet" && p2pick)
            {
                p2pick = false;
                Ship3("P2");
                MoveAudio();
            }
            if ((Input.GetAxis("P2_Horizontal") > 0.5) && p2state == "Scorpion" && p2pick)
            {
                p2pick = false;
                Ship3("P2");
                MoveAudio();
            }
            if ((Input.GetAxis("P2_Horizontal") < -0.5) && p2state == "Scorpion" && p2pick)
            {
                p2pick = false;
                Ship1("P2");
                MoveAudio();
            }
            if ((Input.GetAxis("P2_Horizontal") > 0.5) && p2state == "Viper" && p2pick)
            {
                p2pick = false;
                Ship1("P2");
                MoveAudio();
            }
            if ((Input.GetAxis("P2_Horizontal") < -0.5) && p2state == "Viper" && p2pick)
            {
                p2pick = false;
                Ship2("P2");
                MoveAudio();
            }
        }

        if (isPlayingP1Audio && nowPlayingAudio == false)
        {
            isPlayingP1Audio = false;
            StartCoroutine(p1Audio());
        }

        if (isPlayingP2Audio && nowPlayingAudio == false)
        {
            isPlayingP2Audio = false;
            StartCoroutine(p2Audio());
        }

        //player 1 select
        if ((Input.GetButton("P1_Fire1")) && state == "Hornet")
        {
            GameController.whichShip = 0;
            shipName1.text = "Hornet";
            isPlayingP1Audio = true;
            state = "Selected";
            stateGlobal = state;
        }
        if ((Input.GetButton("P1_Fire1")) && state == "Scorpion")
        {
            GameController.whichShip = 1;
            shipName1.text = "Scorpion";
            isPlayingP1Audio = true;
            state = "Selected";
            stateGlobal = state;
        }
        if ((Input.GetButton("P1_Fire1")) && state == "Viper")
        {
            GameController.whichShip = 2;
            shipName1.text = "Viper";
            isPlayingP1Audio = true;
            state = "Selected";
            stateGlobal = state;
        }

        if (players == 2)
        {
            //Player2 select
            if ((Input.GetButton("P2_Fire1")) && p2state == "Hornet")
            {
                GameController.whichShip2 = 0;
                shipName2.text = "Hornet";
                isPlayingP2Audio = true;
                p2state = "Selected";
                stateGlobal2 = p2state;
            }
            if ((Input.GetButton("P2_Fire1")) && p2state == "Scorpion")
            {
                GameController.whichShip2 = 1;
                shipName2.text = "Scorpion";
                isPlayingP2Audio = true;
                p2state = "Selected";
                stateGlobal2 = p2state;
            }
            if ((Input.GetButton("P2_Fire1")) && p2state == "Viper")
            {
                GameController.whichShip2 = 2;
                shipName2.text = "Viper";
                isPlayingP2Audio = true;
                p2state = "Selected";
                stateGlobal2 = p2state;
            }
        }
    }
    void MoveAudio()
    {
        volume = Random.Range(0.2f, 0.5f);
        source.PlayOneShot(Move, volume);
    }
    void Ship1(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            ship = "Hornet";
            shipType = "Defense fighter";
            health = 100;
            speed = 8;
            size = 50;
            power = 150; // use for ability
            weapon1 = "Dual machine gun";
            weapon2 = "Spread Shot";
            weapon3 = "Drones";
            weapon4 = "Super Spread";
            StartCoroutine(PauseShip1("P1"));
            SetText("P1");
        }
        if (playerPrefix == "P2")
        {
            p2ship = "Hornet";
            p2shipType = "Defense fighter";
            p2health = 100;
            p2speed = 8;
            p2size = 50;
            p2power = 150; // use for ability
            p2weapon1 = "Dual machine gun";
            p2weapon2 = "Spread Shot";
            p2weapon3 = "Drones";
            p2weapon4 = "Super Spread";
            StartCoroutine(PauseShip1("P2"));
            SetText("P2");
        }
    }
    void Ship2(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            ship = "Scorpion";
            shipType = "Attack Corvette";
            health = 250;
            speed = 5;
            size = 75;
            power = 100;
            weapon1 = "Quad laser";
            weapon2 = "Tail Laser";
            weapon3 = "Shield";
            weapon4 = "Mega Beam";
            StartCoroutine(PauseShip2("P1"));
            SetText("P1");
        }
        if (playerPrefix == "P2")
        {
            p2ship = "Scorpion";
            p2shipType = "Attack Corvette";
            p2health = 250;
            p2speed = 5;
            p2size = 75;
            p2power = 100;
            p2weapon1 = "Quad laser";
            p2weapon2 = "Tail Laser";
            p2weapon3 = "Shield";
            p2weapon4 = "Mega Beam";
            StartCoroutine(PauseShip2("P2"));
            SetText("P2");
        }
    }
    void Ship3(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            ship = "Viper";
            shipType = "Stealth fighter";
            health = 50;
            speed = 12;
            size = 25;
            power = 200;
            weapon1 = "Dual machine gun";
            weapon2 = "Tripple Missile";
            weapon3 = "Dodge";
            weapon4 = "Missile Barrage";
            StartCoroutine(PauseShip3("P1"));
            SetText("P1");
        }
        if (playerPrefix == "P2")
        {
            p2ship = "Viper";
            p2shipType = "Stealth fighter";
            p2health = 50;
            p2speed = 12;
            p2size = 25;
            power = 200;
            p2weapon1 = "Dual machine gun";
            p2weapon2 = "Tripple Missile";
            p2weapon3 = "Dodge";
            p2weapon4 = "Missile Barrage";
            StartCoroutine(PauseShip3("P2"));
            SetText("P2");
        }
    }
    void SetText(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            Info.text = "Character selection: Player 1" + "\n\nShip: " + ship + ", " + shipType + "\nWeapons: \nPut weapon images here";
            Info2.text = "Health: \nSpeed: \nSize: \nPower: \nOverall: ";
            p1Bars[0].fillAmount = (health / 2) / healthBarMax;
            p1Bars[1].fillAmount = (speed * 10) / speedBarMax;
            p1Bars[2].fillAmount = size / sizeBarMax;
            p1Bars[3].fillAmount = (power / 2) / powerBarMax;
            p1Bars[4].fillAmount = ((health / 2) + speed + size + (power / 2)) / overallBarMax;
        }
        if (playerPrefix == "P2")
        {
            p2Info.text = "Character selection: Player 2" + "\n\nShip: " + p2ship + ", " + p2shipType + "\nWeapons: \nPut weapon images here";
            p2Info2.text = "Health: \nSpeed: \nSize: \nPower: \nOverall: ";
            p2Bars[0].fillAmount = (p2health / 2) / healthBarMax;
            p2Bars[1].fillAmount = (p2speed * 10) / speedBarMax;
            p2Bars[2].fillAmount = p2size / sizeBarMax;
            p2Bars[3].fillAmount = (p2power / 2) / powerBarMax;
            p2Bars[4].fillAmount = ((p2health / 2) + p2speed + p2size + (p2power / 2)) / overallBarMax;
        }
    }
    IEnumerator p1Audio()
    {
        nowPlayingAudio = true;
        if (GameController.whichShip == 0)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Hornet, volume);
        }
        else if (GameController.whichShip == 1)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Scorpion, volume);
        }
        else if (GameController.whichShip == 2)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Viper, volume);
        }
        yield return new WaitForSeconds(1);
        nowPlayingAudio = false;
    }
    IEnumerator p2Audio()
    {
        nowPlayingAudio = true;
        if (GameController.whichShip2 == 0)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Hornet, volume);
        }
        else if (GameController.whichShip2 == 1)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Scorpion, volume);
        }
        else if (GameController.whichShip2 == 2)
        {
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(Viper, volume);
        }
        yield return new WaitForSeconds(1);
        nowPlayingAudio = false;
    }
    IEnumerator PauseShip1(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            state = ship;
            p1ShipImage.sprite = ships[0];
        }
        if (playerPrefix == "P2")
        {
            p2state = p2ship;
            p2ShipImage.sprite = ships[0];
        }
        yield return new WaitForSeconds(0.1f);
        if (playerPrefix == "P1")
        {
            pick = true;
        }
        if (playerPrefix == "P2")
        {
            p2pick = true;
        }
    }
    IEnumerator PauseShip2(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            state = ship;
            p1ShipImage.sprite = ships[1];
        }
        if (playerPrefix == "P2")
        {
            p2state = p2ship;
            p2ShipImage.sprite = ships[1];
        }
        yield return new WaitForSeconds(0.1f);
        if (playerPrefix == "P1")
        {
            pick = true;
        }
        if (playerPrefix == "P2")
        {
            p2pick = true;
        }
    }
    IEnumerator PauseShip3(string playerPrefix)
    {
        if (playerPrefix == "P1")
        {
            state = ship;
            p1ShipImage.sprite = ships[2];
        }
        if (playerPrefix == "P2")
        {
            p2state = p2ship;
            p2ShipImage.sprite = ships[2];
        }
        yield return new WaitForSeconds(0.1f);
        if (playerPrefix == "P1")
        {
            pick = true;
        }
        if (playerPrefix == "P2")
        {
            p2pick = true;
        }
    }
    IEnumerator LoadWait()
    {
        yield return new WaitForSeconds(3);
        ao.allowSceneActivation = true;
        SceneManager.LoadScene("Demo");
    }
}
