using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContactPhysical : MonoBehaviour {
    // Update is called once per frame
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject playerExplosion;
    [SerializeField]
    private int scoreValue;
    private GameController gameController;
    [SerializeField]
    private int damage;
    private int pHealth;
    private int pShield;
    [SerializeField]
    private int health;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyLaser") || other.CompareTag("PowerUp") || other.CompareTag("Explosion"))
        {
            return;
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
            health -= damage;
            if (other.CompareTag("PlayerShield"))
            {
                GameController.playerShield = pShield;
            }
            else if (other.CompareTag("Player2Shield"))
            {
                GameController.player2Shield = pShield;
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
                health -= damage;
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
        if (health <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
