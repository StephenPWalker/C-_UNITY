using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] music;
    private AudioClip musicTrack;
    [SerializeField]
    private GameObject warning;
    public GameObject[] PlayerShips;
    public int[] ShipHealth;
    public int[] ShipShield;
    public Vector3[] startPositions;
    public Vector3[] midPositions;
    public Vector3[] endPositions;
    public GameObject[] FormationShips;
    public GameObject[] Hazards;
    public GameObject[] TopShips;
    public GameObject Bound;
    public GameObject Nebula;
    public GameObject pauseScreen;
    public Transform BoundPos;
    public Transform NebulaPos;
    public Transform p1pos;
    public Transform p2pos;

    public Vector3[] spawnValues;
    public Vector3[] spawnSideValues;

    private Boundary boundary;

    public float spawnWait;
    public float startWait;
    public float waveWait;

    private string shotType;
    private string playerPrefix;
    public static string powerName;

    private int howManyPlayers;
    private int waveNumber;
    private int waveSpawn;
    private int waveCount;
    private int score;
    private int score2;
    private float timeOut;
    private float nebulaSpawnTimer;

    public Text scoreText;
    public Text scoreText2;
    public Text restartText;
    public Text gameOverText;
    public Text p1ContinueText;
    public Text p1ContinueText2;
    public Text p2ContinueText;
    public Text p2ContinueText2;
    public Image scoreChart;
    public Image scoreChart2;
    public Text scoreInfoTitle;
    public Text scoreInfoTitle2;
    public Text scoreInfo;
    public Text scoreInfo2;

    public static int continues;
    public static int playerHealth;
    public static int playerShield;
    public static int player2Health;
    public static int player2Shield;

    public static int playerHealthMax;
    public static int playerShieldMax;
    public static int player2HealthMax;
    public static int player2ShieldMax;

    public int mBossHealth;
    public int mBossHealthMax;
    public static int bossHealth;
    public static int bossHealthMax;
    public static bool bossOn = false;

    public static bool delayActive = false;
    public static bool delayOver = false;
    public static bool isLevelOver = false;

    private bool wantToContinue;
    private bool gameOver;
    private bool restart;
    private bool Alive = true;

    private bool p1SpawnCountdown = false;
    private bool p2SpawnCountdown = false;

    public string Level;
    public string Menu;

    public static int whichShip = 0;
    public static int whichShip2 = 0;
    public static int Counter = 0;
    private AudioSource source;

    float pauseStart;
    float timeNow;
    float pauseEnd;

    private void Start()
    {
        Instantiate(Nebula, NebulaPos.position, NebulaPos.rotation);
        nebulaSpawnTimer = 90;
        musicTrack = music[0];
        source = GetComponent<AudioSource>();
        source.clip = musicTrack;
        source.Play();

        scoreChart.color = new Color(scoreChart.color.r, scoreChart.color.g, scoreChart.color.b, 0);
        scoreChart2.color = new Color(scoreChart.color.r, scoreChart.color.g, scoreChart.color.b, 0);
        timeOut = 10;

        //alter for start wave  BOSS = 23
        //alter for start wave MiniBoss = 9
        //alter for start wave start = 1
        waveCount = 1;
        /*
        shotType = "Dual Shot";
        powerName = "None";
        */


        // should be set to 3 for normal
        continues = 3;

        howManyPlayers = CharacterSelect.howManyPlayers;

        P1();
        if (howManyPlayers == 2)
        {
            P2();
        }


        bossHealth = mBossHealth;
        bossHealthMax = mBossHealthMax;

        waveNumber = 1;
        wantToContinue = false;
        isLevelOver = false;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";

        score = 0;

        UpdateScore();
        StartCoroutine(SpawnWaves(3));
        source = GetComponent<AudioSource>();
        source.Play();
    }

    private void P1()
    {
        Instantiate(PlayerShips[whichShip], p1pos.position, p1pos.rotation);
        playerHealth = ShipHealth[whichShip];
        playerShield = ShipShield[whichShip];
        playerHealthMax = ShipHealth[whichShip];
        playerShieldMax = ShipShield[whichShip];
    }
    private void P2()
    {
        Instantiate(PlayerShips[whichShip2], p2pos.position, p2pos.rotation);
        player2Health = ShipHealth[whichShip2];
        player2Shield = ShipShield[whichShip2];
        player2HealthMax = ShipHealth[whichShip2];
        player2ShieldMax = ShipShield[whichShip2];
    }

    private void Update()
    {

        timeNow = Time.realtimeSinceStartup;
        restartText.text = "Wave: " + waveCount;
        if (PlayerController.isGamePaused == false && (Input.GetButton("P1_Menu") || Input.GetButton("P2_Menu")) && timeNow - pauseEnd > 1)
        {
            pauseStart = Time.realtimeSinceStartup;
            Time.timeScale = 0;
            source.Pause();
            pauseScreen.SetActive(true);
            PlayerController.isGamePaused = true;
        }
        else if(PlayerController.isGamePaused && (Input.GetButton("P1_Menu") || Input.GetButton("P2_Menu")) && timeNow - pauseStart > 1)
        {
            Time.timeScale = 1;
            source.Play();
            pauseScreen.SetActive(false);
            PlayerController.isGamePaused = false;
            pauseEnd = Time.realtimeSinceStartup;
        }

        if (continues > 0)
        {
                p1ContinueText.text = "Lives: " + continues;
                if (CharacterSelect.howManyPlayers == 2)
                {
                    p2ContinueText.text = "Lives: " + continues;
                }
                else
                {
                    p2ContinueText.text = " ";
                }
        }
        else if(continues <= 0 && (playerHealth <= 0 && player2Health <= 0))
        {
            Alive = false;
            p1ContinueText.text = " ";
            p2ContinueText.text = " ";
        }
          
        if (playerHealth <= 0 || player2Health <= 0)
        {
            WantToContinue();
        }

        if (Alive == false)
        {
            GameOver();
        }
        if (!bossOn)
        {
            nebulaSpawnTimer -= Time.deltaTime;
            if (nebulaSpawnTimer <= 0)
            {
                Instantiate(Nebula, NebulaPos.position, NebulaPos.rotation);
                nebulaSpawnTimer = 86;
            }
        }
        GameObject boundaryObject = GameObject.FindWithTag("Boundary");

        if (boundaryObject == null)
        {
        Instantiate(Bound, BoundPos.position, BoundPos.rotation);
        }

        if (delayOver) //called on by destroyByContact script
        {
            Resume();
        }
        /*
        if (powerName != "None")
        {
            PowerUp();
        }
        */
        if (restart)
        {
            Counter = 0; //defines p1 p2
            if (Input.GetButton("P1_Menu") || Input.GetButton("P2_Menu"))
            {
                Alive = true;
                isLevelOver = false;
                bossOn = false;
                delayOver = false;
                delayActive = false;
                wantToContinue = false;
                gameOver = false;
                restart = false;
                SceneManager.LoadScene(Level);
            }
        }

        //This is for game over, mentioned for both players?
        if (gameOver)
        {
            timeOut -= Time.deltaTime;
            restartText.text = "Press menu button for Restart" + "\nTime: " + (int)timeOut;
            restart = true;
            bossOn = false;
            if (timeOut <= 0)
            {
                SceneManager.LoadScene(Menu);
            }
        }
        //Both continues working correctly, check health for after continue!
        if (wantToContinue && continues > 0)
        {           
            if (Input.GetButton("P1_Fire2") && playerHealth <= 0)
            {
                SpawnP1();
            }

            if (Input.GetButton("P2_Fire2") && player2Health <= 0 && CharacterSelect.howManyPlayers == 2)
            {
                SpawnP2();
            }
        }
        if (bossHealth <= 0)
        {
            //Need to add a score breakdown at this point.
            bossOn = false;
            levelOver();
        }
    }
    void Resume()
    {
        delayOver = false;
        delayActive = false;
        StartCoroutine(SpawnWaves(3));
    }
    //spawn player 1
    void SpawnP1()
    {
        continues -= 1;
        playerHealth = ShipHealth[whichShip];
        playerHealthMax = ShipHealth[whichShip];
        playerShield = ShipShield[whichShip];
        playerShieldMax = ShipShield[whichShip];
        Counter = 0; //defines p1 p2
        Instantiate(PlayerShips[whichShip], p1pos.position, p1pos.rotation);
        wantToContinue = false;
        p1SpawnCountdown = false;
        p1ContinueText2.text = " ";
    }
    //spawn player 2
    void SpawnP2()
    {
        continues -= 1;
        player2Health = ShipHealth[whichShip2];
        player2HealthMax = ShipHealth[whichShip2];
        player2Shield = ShipShield[whichShip2];
        player2ShieldMax = ShipShield[whichShip2];
        Counter = 1; //defines p1 p2
        Instantiate(PlayerShips[whichShip2], p2pos.position, p2pos.rotation);
        wantToContinue = false;
        p2SpawnCountdown = false;
        p2ContinueText2.text = " ";
    }
    //increase the wave each wave
    void IncreaseWave()
    {
        if (Alive)
        { 
            // increases wave number and wave number text spawn dead players
            waveNumber += 1;
        }
    }
    public void AddScore(int newScoreValue)
    {
        //updates score
        if (playerHealth > 0)
        {
            score += newScoreValue;
        }
        UpdateScore();
    }
    public void AddScore2(int newScoreValue)
    {
        //updates score
        if (player2Health > 0)
        {
            score2 += newScoreValue;
        }
        UpdateScore();
    }
    void UpdateScore()
    {
        //updates score text
        scoreText.text = "P1 Score:\n" + score;
        if (CharacterSelect.howManyPlayers == 2)
        {
            scoreText2.text = "P2 Score:\n" + score2;
        }
        else
        {
            scoreText2.text = " ";
        }
    }
    //game over text
    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }
    //if playyer is dead
    public void WantToContinue()
    {
        if (playerHealth <= 0 && p1SpawnCountdown == false)
        {
            wantToContinue = true;
            StartCoroutine(ForceSpawn());
        }
        if (player2Health <= 0 && CharacterSelect.howManyPlayers == 2 && p2SpawnCountdown == false)
        {
            wantToContinue = true;
            StartCoroutine(ForceSpawn2());
        }
    }
    public void levelOver()
    {      
        // at this point the level should end and the score should be tallied
        if (playerHealth > 0)
        {
            isLevelOver = true;
            StartCoroutine(ContinueToWarp());
            /*
             * At this point we want the score table to appear, 2p from each side, 1p from one side, details to be included are
             * total score, tally
             * bonus score for bombs remaining, health, lives left, and difficulty
             */
            scoreChart.color = new Color(scoreChart.color.r, scoreChart.color.g, scoreChart.color.b, 0.75f);
            scoreInfoTitle.text = "Player 1 : Summary and stats";
            scoreInfo.text = "Score           - " + score +
                 "\n\nBonus:" +
                 "\nHealth total  - " + playerHealth * 500 +
                 "\nNormal mode - " + 20000 +
                 "\nLives left (" + continues + ") - " + continues * 1000 +
                 "\nBombs (0)   - " +
                 "\n\nTotal score = " + (score + (playerHealth * 500) + (continues * 1000));
            if (howManyPlayers == 2)
            {
                scoreChart2.color = new Color(scoreChart.color.r, scoreChart.color.g, scoreChart.color.b, 0.75f);
                scoreInfoTitle2.text = "Player 2 : Summary and stats";
                scoreInfo2.text = "Score            - " + score +
                 "\n\nBonus:" +
                 "\nHealth total  - " + player2Health * 500 +
                 "\nNormal mode - " + 20000 +
                 "\nLives left (" + continues + ") - " + continues * 1000 +
                 "\nBombs (0)   - " +
                 "\n\nTotal score = " + (score + (player2Health * 500) + (continues * 1000));
            }
        }
        else
        {
            gameOverText.text = "GAME OVER!";
            gameOver = true;
        }      
    }
    /*
    public void PowerUp()
    {
        //power ups
        if (powerName == "Single Shot")
        {
            shotType = "Single Shot";
            PlayerController.startNumberOfGuns = 0;
            PlayerController.endNumberOfGuns = 1;
            powerName = "None";
        }
        if (powerName == "Dual Shot")
        {
            shotType = "Dual Shot";
            PlayerController.startNumberOfGuns = 1;
            PlayerController.endNumberOfGuns = 3;
            powerName = "None";
        }
        if (powerName == "Tri Shot")
        {
            shotType = "Tri Shot";
            PlayerController.startNumberOfGuns = 0;
            PlayerController.endNumberOfGuns = 3;
            powerName = "None";
        }
        if (powerName == "TriAng Shot")
        {
            shotType = "TriAng Shot";
            PlayerController.startNumberOfGuns = 5;
            PlayerController.endNumberOfGuns = 8;
            powerName = "None";
        }
        if (powerName == "Quad Shot")
        {
            shotType = "Quad Shot";
            PlayerController.startNumberOfGuns = 1;
            PlayerController.endNumberOfGuns = 5;
            powerName = "None";
        }
        if (powerName == "PentAng Shot")
        {
            shotType = "PentAng Shot";
            PlayerController.startNumberOfGuns = 5;
            PlayerController.endNumberOfGuns = 10;
            powerName = "None";
        }
    }
    */
    void WhichWave(string level)
    {
        // the different groups that spawn in each wave
        if (level == "Level1")
        {
            Level1();
        }
        if (level == "Demo")
        {
            levelDemo();
        }
    }
    /* no(which spawn)  (no) pos
    * 0          1 (18) 
    * 
    * 2          3 (14)
    * 
    * 4          5 (8)
    * 
    * 6          7 (4)
    * 
    * 8          9 (0)
    * 
    * 10         11(-2)
    */

    // *   *   *   *   *   *   *   *   *
    // -15 -13 -7  -3  0   3   7   13  15
    //x 0   1   2   3   4   5   6   7   8
    void Level1()
    {
        if (waveSpawn == 1)
        {
            StartCoroutine(SpawnWaves(4));
        }
        if (waveSpawn == 2)
        {
            StartCoroutine(TankWave());
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 3)
        {
            StartCoroutine(TankWave());
            StartCoroutine(TankWave2(2));
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 4)
        {
            StartCoroutine(StrikerWave());
            StartCoroutine(TankWave2(2));
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 5)
        {
            StartCoroutine(StrikerWave());
            // Tower turrets in the base
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 6)
        {
            StartCoroutine(WaveMoveFromSide(0, 2, 3, 4));
            StartCoroutine(WaveMoveFromSide(0, 3, 3, 5));
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 7)
        {
            StartCoroutine(TankWave());
            StartCoroutine(TankWave2(2));
            StartCoroutine(StrikerWave());
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 8)
        {
            StartCoroutine(WaveMoveFromSide(0, 4, 4, 2));
            StartCoroutine(WaveMoveFromSide(0, 5, 4, 3));
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 9)
        {
            StartCoroutine(TankWave());
            StartCoroutine(WaveMoveFromSide(0, 4, 3, 2));
            StartCoroutine(WaveMoveFromSide(0, 5, 3, 3));
            StartCoroutine(SpawnWaves(6));
        }
        if (waveSpawn == 10)
        {
            StartCoroutine(TankWave2(3));
            StartCoroutine(WaveMoveFromTopDirected(1, 2, 3, 6));
            StartCoroutine(WaveMoveFromTopDirected(1, 6, 3, 7));
            StartCoroutine(SpawnWaves(8));
        }
        if (waveSpawn == 11)
        {
            StartCoroutine(WaveMoveFromTopDirected(1, 2, 3, 6));
            StartCoroutine(WaveMoveFromTopDirected(1, 6, 3, 7));
            StartCoroutine(SpawnWaves(8));
        }
        if (waveSpawn == 12)
        {
            StartCoroutine(WaveMoveFromSide(0, 4, 4, 2));
            StartCoroutine(WaveMoveFromSide(0, 5, 4, 3));
            StartCoroutine(SpawnWaves(8));
        }
        if (waveSpawn == 13)
        {
            StartCoroutine(StrikerWave());
            //AA turret territory
            StartCoroutine(SpawnWaves(12));
        }
        if (waveSpawn == 14)
        {
            StartCoroutine(WaveMoveFromTop(3, 10, 1));
            StartCoroutine(TankWave2(7));
            StartCoroutine(SpawnWaves(18));
        }
    }
    void levelDemo()
    {
        //squads
        if (waveSpawn == 1 || waveSpawn == 16)
        {
            StartCoroutine(StrikerSquadrenWave());
            StartCoroutine(SpawnWaves(4));
        }
        //high cross
        if (waveSpawn == 2)
        {
            StartCoroutine(WaveMoveFromSide(0, 0, 5, 0));
            StartCoroutine(WaveMoveFromSide(0, 1, 5, 1));
            StartCoroutine(SpawnWaves(3));
        }
        //mid cross
        if (waveSpawn == 3)
        {
            StartCoroutine(WaveMoveFromSide(0, 0, 5, 2));
            StartCoroutine(WaveMoveFromSide(0, 1, 5, 3));
            StartCoroutine(SpawnWaves(3));
        }
        //turret only
        if (waveSpawn == 4)
        {
            StartCoroutine(TurretWave());
            StartCoroutine(SpawnWaves(7));
        }
        //mine only
        if (waveSpawn == 5)
        {
            StartCoroutine(MineWave1());
            StartCoroutine(SpawnWaves(7));
        }
        //low cross high cross
        if (waveSpawn == 6)
        {
            StartCoroutine(WaveMoveFromSide(0, 0, 3, 0));//high
            StartCoroutine(WaveMoveFromSide(0, 1, 3, 1));//high
            StartCoroutine(WaveMoveFromSide(1, 4, 3, 4));//low
            StartCoroutine(WaveMoveFromSide(1, 5, 3, 5));//low
            StartCoroutine(SpawnWaves(3));
        }
        //charger
        if (waveSpawn == 7)
        {
            StartCoroutine(TurretWave());
            StartCoroutine(WaveMoveFromTop(0, 2, 3));
            StartCoroutine(WaveMoveFromTop(0, 6, 3));
            StartCoroutine(SpawnWaves(3));
        }
        //loop in and up
        if (waveSpawn == 8)
        {
            StartCoroutine(WaveMoveFromTopDirected(0, 1, 4, 6));
            StartCoroutine(WaveMoveFromTopDirected(0, 7, 4, 7));
            StartCoroutine(TurretWaveSides(1));
            StartCoroutine(SpawnWaves(7));
        }
        // low high cross
        if (waveSpawn == 9)
        {
            StartCoroutine(WaveMoveFromSide(0, 0, 3, 0));//high
            StartCoroutine(WaveMoveFromSide(0, 1, 3, 1));//high
            StartCoroutine(WaveMoveFromSide(1, 4, 3, 4));//low
            StartCoroutine(WaveMoveFromSide(1, 5, 3, 5));//low
            StartCoroutine(SpawnWaves(3));
        }

        if (waveSpawn == 10)
        {
            StartCoroutine(WaveMoveFromSide(0, 6, 3, 6));
            StartCoroutine(WaveMoveFromSide(0, 7, 3, 7));
            StartCoroutine(WaveMoveFromSide(1, 4, 3, 8));
            StartCoroutine(WaveMoveFromSide(1, 5, 3, 9));
            // loop centre waves
            StartCoroutine(SpawnWaves(3));
        }
        if (waveSpawn == 11)
        {
            //gunship should be made here
            StartCoroutine(SpawnWaves(0));
        }
        // loop de loop squids
        if (waveSpawn == 12 || waveSpawn == 18)
        {
            StartCoroutine(WaveMoveFromTopDirected(1, 1, 7, 6));
            StartCoroutine(WaveMoveFromTopDirected(1, 7, 7, 7));
            StartCoroutine(TurretWaveSides(1));
            StartCoroutine(SpawnWaves(5));
        }
        // side turrets
        if (waveSpawn == 13)
        {
            StartCoroutine(TurretWaveSides(2));
            StartCoroutine(SpawnWaves(5));
        }
        // charger wave plus turrets
        if (waveSpawn == 14 || waveSpawn == 19)
        {
            StartCoroutine(WaveMoveFromTop(0, 2, 5));
            StartCoroutine(WaveMoveFromTop(0, 6, 5));
            StartCoroutine(TurretWaveSides(1));
            StartCoroutine(SpawnWaves(3));
        }
        //miniboss
        if (waveSpawn == 15)
        {
            StartCoroutine(miniBoss());
        }
        //turrets side and mines small
        if (waveSpawn == 17)
        {
            StartCoroutine(TurretWaveSides(2));
            StartCoroutine(MineWave1());
            StartCoroutine(SpawnWaves(7));
        }
        if (waveSpawn == 20)
        {
            StartCoroutine(WaveMoveFromTopDirected(2, 1, 7, 6));
            StartCoroutine(WaveMoveFromTopDirected(2, 7, 7, 7));
            StartCoroutine(TurretWaveSides(2));
            StartCoroutine(SpawnWaves(7));
        }
        if (waveSpawn == 21)
        {
            // here want 2 gunships criss crossing
            StartCoroutine(SpawnWaves(0));
        }
        if (waveSpawn == 22)
        {
            StartCoroutine(WaveMoveFromTop(0, 2, 7));
            StartCoroutine(WaveMoveFromTop(0, 6, 7));
            StartCoroutine(MineWave2());
            StartCoroutine(SpawnWaves(7));
        }
        if (waveSpawn == 23)
        {
            StartCoroutine(BossSpawnTime());
        }
    }
    /* Spawner and disablers
    */
    IEnumerator BossSpawnTime()
    {
        if (!warning.GetComponent<FadeInOut>().getIsActive())
            warning.GetComponent<FadeInOut>().setIsActive();
        while (source.volume > 0)
        {
            source.volume -= (1 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(boss());
        musicTrack = music[1];
        source.clip = musicTrack;
        source.Play();
        while (source.volume < 0.75)
        {
            source.volume += (1 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator ForceSpawn()
    {
        p1SpawnCountdown = true;
        int time = 10;
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            if (playerHealth <= 0 && continues >= 1)
                p1ContinueText2.text = "Spawning in " + time;
        }
        p1ContinueText2.text = " ";
        if (playerHealth <= 0 && continues >= 1)
        {
            if(time == 0)
                SpawnP1();
        }
    }
    IEnumerator ForceSpawn2()
    {
        p2SpawnCountdown = true;
        int time2 = 10;
        while (time2 > 0)
        {
            yield return new WaitForSeconds(1);
            time2--;
            if (player2Health <= 0 && continues >= 1)
                p2ContinueText2.text = "Spawning in " + time2;
        }
        p2ContinueText2.text = " ";
        if (player2Health <= 0 && continues >= 1)
        {
            if (time2 == 0)
                SpawnP2();
        }
    }
    IEnumerator ContinueToWarp()
    {
        yield return new WaitForSeconds(10);
        gameOverText.text = "PRESS\nFIRE";
    }
    /* 
     * Spawn Waves
     */
    IEnumerator SpawnWaves(int delay)
    {
        yield return new WaitForSeconds(delay);
        if (delayActive == false)
        {
            if (Alive)
            {

                yield return new WaitForSeconds(waveWait);
                waveSpawn = waveCount;
                WhichWave(Level);
                IncreaseWave();
                waveCount += 1;
            }
        }
    }

    /* 
     * BOSSES
     */
    IEnumerator boss()
    {
        yield return new WaitForSeconds(spawnWait * 2);
        WaveLogic(5, 9);
        bossOn = true;
    }
    /* 
     * MINI-BOSSES
     */
    IEnumerator miniBoss()
    {
        yield return new WaitForSeconds(spawnWait * 2);
        WaveLogic(3, 9);     
    }
    /* 
     * Enemy Ships
     */
    /* no(which spawn)  (no) pos
    * 0          1 (18) 
    * 
    * 2          3 (14)
    * 
    * 4          5 (8)
    * 
    * 6          7 (4)
    * 
    * 8          9 (0)
    * 
    * 10         11(-2)
    */

    // *   *   *   *   *   *   *   *   *
    // -15 -13 -7  -3  0   3   7   13  15
    //x 0   1   2   3   4   5   6   7   8
    // For basic mines wave complete
    IEnumerator StrikerWave()
    {
        WaveLogic(2, 4);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(2, 1);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(2, 7);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(2, 3);
    }
    IEnumerator StrikerSquadrenWave()
    {
        WaveLogic(0, 4);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 3);
        WaveLogic(0, 5);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 1);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 0);
        WaveLogic(0, 2);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 7);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 6);
        WaveLogic(0, 8);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 4);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 3);
        WaveLogic(0, 5);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 1);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 0);
        WaveLogic(0, 2);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 7);
        yield return new WaitForSeconds(0.5f);
        WaveLogic(0, 6);
        WaveLogic(0, 8);
        yield return new WaitForSeconds(0.5f);
    }
    /* 
     * Enemy Tanks
     */
    IEnumerator TankWave()
    {
        TankLogic(0, 4, "Right", true);
        TankLogic(0, 7, "Left", true);
        yield return new WaitForSeconds(spawnWait * 3);
        TankLogic(0, 4, "Down", false);
        TankLogic(0, 6, "Down", false);
        yield return new WaitForSeconds(spawnWait * 3);
        TankLogic(0, 3, "Down", false);
        TankLogic(0, 5, "Down", false);
    }
    IEnumerator TankWave2(int waves)
    {
        for (int i = 0; i < waves; i++)
        {
            TankLogic(0, 2, "Right", true);
            TankLogic(0, 3, "Left", true);
            yield return new WaitForSeconds(spawnWait * 3);
        }
    }
    /* 
     * Enemy Turrets
     */
    IEnumerator TurretWaveSides(int count)
    {
        if (count == 1)
        {
            WaveLogic(1, 1);
            yield return new WaitForSeconds(spawnWait * 2);
            WaveLogic(1, 7);
        }
        else if (count == 2)
        {
            WaveLogic(1, 7);
            yield return new WaitForSeconds(spawnWait * 2);
            WaveLogic(1, 1);
        }
    }
    IEnumerator TurretWave()
    {
        WaveLogic(1, 1);
        WaveLogic(1, 7);
        yield return new WaitForSeconds(spawnWait * 2);
        WaveLogic(1, 2);
        WaveLogic(1, 6);
        yield return new WaitForSeconds(spawnWait * 2);
    }

    /* 
     * Enemy Mines
     */
    IEnumerator MineWave1()
    {
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 0);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 2);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 4);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 3);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 1);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 5);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 6);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 8);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 7);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 0);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 2);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 4);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 6);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 3);
        yield return new WaitForSeconds(spawnWait);
    }
    IEnumerator MineWave2()
    {
        // For large mines wave incomplete
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 0);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 2);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 4);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 3);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 1);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 5);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 6);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 8);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 7);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 0);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 2);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 5);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(4, 6);
        yield return new WaitForSeconds(spawnWait);
        WaveLogic(2, 3);
        yield return new WaitForSeconds(spawnWait);
    }

    /* 
     * Enumerated waves
     */
    IEnumerator WaveMoveFromSide(int shipNo, int whichSpawn, int noOfShips,int sequence)
    {
        for (int i = 0; i < noOfShips; i++)
        {   
            yield return new WaitForSeconds(spawnWait);
            WaveLogicFormation(shipNo, whichSpawn, sequence, true);
        }
    }
    IEnumerator WaveMoveFromTopDirected(int shipNo, int whichSpawn, int noOfShips, int sequence)
    {
        for (int i = 0; i < noOfShips; i++)
        {
            yield return new WaitForSeconds(spawnWait * 2);
            WaveLogicFormation(shipNo, whichSpawn, sequence, false);
        }
    }
    //  *   *   *   *   *   *   *   *   *
    // -15 -13 -7  -3   0   3   7   13  15  (pos)
    //x 0   1   2   3   4   5   6   7   8   (whichSpawn)
    IEnumerator WaveMoveFromTop(int shipNo, int whichSpawn, int noOfShips)
    {
        for (int i = 0; i < noOfShips; i++)
        {
            yield return new WaitForSeconds(spawnWait * 2);
            WaveLogic(shipNo, whichSpawn);
        }
    }

    /* 
     * Wave voids
     */
    void WaveLogic(int whichHazard, int position)
    {
        //For top down hazards
        GameObject hazard = Hazards[whichHazard];
        Vector3 spawnPosition = new Vector3(spawnValues[position].x, spawnValues[position].y, spawnValues[position].z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(hazard, spawnPosition, spawnRotation);
    }
    void TankLogic(int whichHazard, int position, string direction, bool isSides)
    {
        //For top down hazards
        Vector3 spawnPosition;
        Quaternion spawnRotation = Quaternion.identity;
        GameObject hazard = Hazards[whichHazard];
        if (isSides)
        {
            spawnPosition = new Vector3(spawnSideValues[position].x, spawnSideValues[position].y, spawnSideValues[position].z);
        }
        else
        {
            spawnPosition = new Vector3(spawnValues[position].x, spawnValues[position].y, spawnValues[position].z);
        }
        hazard.GetComponentInChildren<Movement>().SetDirection(direction);
        Instantiate(hazard, spawnPosition, spawnRotation);
    }
    void WaveLogicFormation(int whichHazard, int position, int sequence, bool isSides)
    {
        //for side accross hazards
        Vector3 spawnPosition;
        GameObject formation = FormationShips[whichHazard];
        if (isSides)
        {
            spawnPosition = new Vector3(spawnSideValues[position].x, spawnSideValues[position].y, spawnSideValues[position].z);
        }
        else
        {
            spawnPosition = new Vector3(spawnValues[position].x, spawnValues[position].y, spawnValues[position].z);
        }
        Quaternion spawnRotation = Quaternion.identity;
        formation.GetComponent<Movement2>().setStart(startPositions[sequence]);
        formation.GetComponent<Movement2>().setMid(midPositions[sequence]);
        formation.GetComponent<Movement2>().setEnd(endPositions[sequence]);
        Instantiate(formation, spawnPosition, spawnRotation);
    }
}
