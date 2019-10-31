using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContactLazer : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;
    public int damage;
    private int pHealth;
    private int pShield;

    private void Start()
    {
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
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Laser") || other.CompareTag("EnemyLaser") || other.CompareTag("PowerUp") || other.CompareTag("Explosion") || other.CompareTag("Boss") || other.CompareTag("Untagged") || other.CompareTag("terrain") || other.CompareTag("BossArm"))
        {
            return;
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        pShield = 0;
        if (other.CompareTag("PlayerShield") && PlayerController.isShieldOn)
        {
            pShield = GameController.playerShield;
        }
        else if (other.CompareTag("Player2Shield") && PlayerController.isShield2On)
        {
            pShield = GameController.player2Shield;
        }
        if (pShield > 0)
        {
            pShield -= damage;
            if (other.CompareTag("PlayerShield"))
                    {
                        GameController.playerShield = pShield;
                    }
            else if (other.CompareTag("Player2Shield"))
                    {
                        GameController.player2Shield = pShield;
                    }
            if (pShield > 0)
            {
                Destroy(gameObject);
            }
            else
            {
                damage += pShield;
            }
            if (damage < 0)
            {
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Player"))
        {
            pHealth = GameController.playerHealth;
        }
        else if (other.CompareTag("Player2"))
        {
            pHealth = GameController.player2Health;
        }
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
                if (pHealth > 0)
                {
                    Instantiate(explosion, other.transform.position, other.transform.rotation);
                    pHealth -= damage;
                if (other.CompareTag("Player"))
                {
                    GameController.playerHealth = pHealth;
                }
                if (other.CompareTag("Player2"))
                {
                    GameController.player2Health = pHealth;
                }
                if (pHealth <= 0)
                    {
                        Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                        Destroy(other.gameObject);
                        if (GameController.continues > 0)
                        {
                            gameController.WantToContinue();
                        }
                }
                    Destroy(gameObject);
                }
            else
            {
                if (GameController.continues > 0)
                {
                    gameController.WantToContinue();
                }
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            }
        }
    }
}

