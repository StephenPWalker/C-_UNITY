using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponControllerMiniboss : MonoBehaviour
{

    public GameObject shot;
    public GameObject shot2;
    public Transform[] shotSpawns2;
    public Transform[] shotSpawns;
    private int shots;
    private int shots2;
    private bool reheat = false;
    private bool reheat2 = false;
    private float waitTimer;
    private float fireRate = 0.1f;
    private int health;
    public int maxHealth;
    private bool phase1 = true;
    private bool phase2 = false;

    // Update is called once per frame
    private void Start()
    {
        shots = 4;
        shots2 = 1;
    }
    void Update()
    {
        health = DestroyByContact.mHealth;
        if (phase1)
        {
            Phase1();
        }
        if (health <= ( maxHealth / 2))
        {
            phase2 = true;
        }
        if (phase2)
        {
            Phase2();
        }
    }
    void Phase1()
    {
        if (shots > 0 && reheat == false)
        {
            StartCoroutine(fire());
        }
        else
        {
            if (reheat == false)
            {
                reheat = true;
                StartCoroutine(reheating());
            }
        }
    }
    void Phase2()
    {
        if (shots2 > 0 && reheat2 == false)
        {
            StartCoroutine(fire2());
        }
        else
        {
            if (reheat2 == false)
            {
                reheat2 = true;
                StartCoroutine(reheating2());
            }
        }
    }

    void Fire()
    {
        for (int i = 0; i < shotSpawns.Length; i++)
        {
            shotSpawns[i].rotation = Quaternion.Euler(0.0f, 180, 0.0f);
            Instantiate(shot, shotSpawns[i].position, shotSpawns[i].rotation);
        }
    }    
    void Fire2()
    {
        for (int i = 0; i < shotSpawns2.Length; i++)
        {
            shotSpawns2[i].rotation = Quaternion.Euler(0.0f, 180, 0.0f);
            Instantiate(shot2, shotSpawns2[i].position, shotSpawns2[i].rotation);
        }
    }
    IEnumerator fire()
    {
        if (Time.time > waitTimer)
        {
            waitTimer = Time.time + fireRate;
            Fire();
            shots -= 1;
        }
        yield return new WaitForSeconds(fireRate);
    }
    IEnumerator fire2()
    {
        if (Time.time > waitTimer)
        {
            waitTimer = Time.time + fireRate;
            Fire2();
            shots2 -= 1;
        }
        yield return new WaitForSeconds(fireRate);
    }

    IEnumerator reheating()
    {
        yield return new WaitForSeconds(6);
        shots = 4;
        reheat = false;
    }
    IEnumerator reheating2()
    {
        yield return new WaitForSeconds(8);
        shots2 = 1;
        reheat2 = false;
    }
}