using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject powerUp;
    private GameController gameController;
    public string power;
    private int lives = 5;

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
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            Instantiate(powerUp, transform.position, transform.rotation);
            GameController.powerName = power;
            Destroy(gameObject);
        }
        else
        {
            lives -= 1;
        }
        if (lives <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
