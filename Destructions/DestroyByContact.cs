using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject damaged;
    public GameObject playerExplosion;
    public GameObject[] powerUps;
    public GameObject asteroid;
    public int scoreValue;
    private GameController gameController;
    private bool bossDestroyed = true;
    public string power;
    public bool isBoss;
    public bool isMiniBoss;
    public bool isAsteroid;
    public bool isShield;
    public bool isTerrain;
    public int spawnAsteroids;
    public int health;
    public static int mHealth;
    private int pHealth;
    private int pShield;
    private bool setDelay = true;
    private int damageFromImpact;

    //want one for player and one for enemies, global one doesn't work for player lazers as they destroy themselves with this code
    //if you can alter this to make that not happen would be ideal
    private void Start()
    {
        mHealth = health;
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((!isBoss && health <= 0 )|| other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyLaser") || other.CompareTag("PowerUp") || other.CompareTag("Explosion") || other.CompareTag("Boss") || other.CompareTag("Untagged") || other.CompareTag("terrain") || other.CompareTag("BossArm"))
        {
            return;
        }
        pShield = 0;
        if (other.CompareTag("PlayerShield") && PlayerController.isShieldOn)
        {
            pShield = GameController.playerShield;
        }
        if (other.CompareTag("Player2Shield") && PlayerController.isShield2On)
        {
            pShield = GameController.player2Shield;
        }
        if (pShield > 0)
        {
            damageFromImpact = pShield;
            pShield -= health;
            if (other.CompareTag("PlayerShield"))
            {
                GameController.playerShield = pShield;
            }
            else if (other.CompareTag("Player2Shield"))
            {
                GameController.player2Shield = pShield;
            }
            health -= damageFromImpact;
            mHealth = health;
        }
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            if (other.CompareTag("Player"))
            {
                pHealth = GameController.playerHealth;
            }
            if (other.CompareTag("Player2"))
            {
                pHealth = GameController.player2Health;
            }
            if (isTerrain)
            {
                pHealth = 0;
            }
            if (pHealth > 0)
            {
                if (health >= 0)
                {
                    damageFromImpact = pHealth;
                    pHealth -= health;
                    if (other.CompareTag("Player"))
                    {
                        GameController.playerHealth = pHealth;
                    }
                    if (other.CompareTag("Player2"))
                    {
                        GameController.player2Health = pHealth;
                    }
                    health -= damageFromImpact;
                    mHealth = health;
                }
                if (isBoss)
                {
                    GameController.bossHealth -= pHealth;
                    pHealth = 0;
                }
                if (pHealth <= 0 && !isTerrain)
                {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    Destroy(other.gameObject);
                    if (GameController.continues > 0)
                    {
                        gameController.WantToContinue();
                    }
                    else
                    {
                        gameController.GameOver();
                    }
                }
                if (health <= 0 && isBoss == false && !isTerrain)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (GameController.continues > 0)
                {
                    gameController.WantToContinue();
                }
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject);
            }
        }

        if (isBoss == true) //only when boss enters
        {
            if (GameController.bossHealth > 0)
            {
                if (!other.CompareTag("Player") && !other.CompareTag("Player2"))
                {
                    GameController.bossHealth -= other.gameObject.GetComponent<Damage>().damage;
                    Instantiate(damaged, other.transform.position, other.transform.rotation);
                    Destroy(other.gameObject);
                    bossDestroyed = false;
                }
            }
            else
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                bossDestroyed = true;
                Destroy(gameObject);
            }
        }

        if ((!other.CompareTag("Player") && !other.CompareTag("Player2")) && (!other.CompareTag("PlayerShield") && !other.CompareTag("Player2Shield")) && isBoss == false && !isTerrain)
        {
            health -= other.gameObject.GetComponent<Damage>().damage;
            mHealth = health;
            Instantiate(damaged, other.transform.position, transform.rotation);
            Destroy(other.gameObject);
        }
        if (health <= 0 && isBoss == false && !isTerrain)
        {
            if (isMiniBoss) // happens when miniBoss dies
            {
                setDelay = true;
                GameController.delayOver = true; //access delayOver in gameController
            }
            if (other.CompareTag("Laser") || other.CompareTag("Projectile"))
            {
                gameController.AddScore(scoreValue);
            }
            if (other.CompareTag("Laser2") || other.CompareTag("Projectile2"))
            {
                gameController.AddScore2(scoreValue);
            }
            if (isAsteroid)
            {
                isAsteroid = false;

                for (int i = 0; i < spawnAsteroids; i++)
                {
                    Vector3 turretPos = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z - i);
                    Instantiate(asteroid, turretPos, transform.rotation);
                }
            }
                Instantiate(explosion, other.transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(gameObject);
        }
        /*
                    int spawnPowerUp = Random.Range(0, 100);
                    if (spawnPowerUp <= 5 && powerUps.Length > 0)
                    {
                        int whichPowerUp = Random.Range(0, powerUps.Length);
                        Instantiate(powerUps[whichPowerUp], other.transform.position, other.transform.rotation);
                        spawnPowerUp = 100;
                    }  
        */
    }
}

