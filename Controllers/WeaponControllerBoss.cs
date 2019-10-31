using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerBoss : MonoBehaviour {

    public GameObject shot;
    public GameObject Secondshot;
    public GameObject Orbshot;
    public Transform[] shotSpawns;
    public Transform[] shotSpawns2;
    public Transform[] shotSpawns3;
    public Transform[] OrbSpawns;
    public Transform[] TrackShotSpawns;
    public Transform lBeam;
    public Transform RBeam;
    public float fireRate;
    public float delay;

    private bool phase1 = false;
    private bool phase2 = false;
    private bool phase3 = false;
    public static bool stage2Active = false;
    public static bool stage3Active = false;
    private int verticalShot = 180;
    private int shots;
    private int shots2;
    private int degreeChange;
    private float waitTimer;
    private float waitTimer2;
    private float waitTimer3;
    private bool Reheat = false;
    private bool Reheat2 = true;
    private bool Reheat3 = false;
    private int orbs = 16;

    private AudioSource source;
    [SerializeField]
    private AudioClip[] sounds;
    private AudioClip sfx;
    private float volLow = 0.2f;
    private float volHigh = 0.5f;
    private float volume;

    private int stage2Count = 2;
    private int stage1Count = 4;

    [SerializeField]
    private TurretWeaponController[] TWC;

    void Start()
    {
        TWC = GetComponentsInChildren<TurretWeaponController>();
        StartCoroutine(WeaponsHeated());
        shots = 8;
    }

    void Update()
    {
        if (phase1)
        {
            Phase1();
        }
        if (phase2)
        {
            Phase2();
        }
        if (phase3)
        {
            Phase3();
        }     
    }
    void Phase1()
    {
        if (shots > 0 && Reheat == false)
        {
            StartCoroutine(fire());
        }
        else
        {
            if (Reheat == false)
            {
                Reheat = true;
                StartCoroutine(pause());
            }
        }

        if (shots2 > 0 && Reheat2 == false)
        {
            StartCoroutine(fire2());
        }
        else
        {
            if (Reheat2 == false)
            {
                Reheat2 = true;
                StartCoroutine(pause2());
            }
        }
    }
    void Phase2()
    {
        if (Reheat3 == false)
        {
            if (orbs > 0)
            {
                StartCoroutine(fire3());
            }
            else
            {
                Reheat3 = true;
                StartCoroutine(pause3());
            }
        }
    }
    void Phase3()
    {
        for (int i = 0; i < TWC.Length; i++)
        {
            if (TWC[i].getInactive() == true && stage3Active)
                TWC[i].setActive();
        }
    }


    void Fire()
    {
        int leftSide = verticalShot - 20;
        int rightSide = verticalShot + 20;
        for (int i = 0; i < shotSpawns.Length; i++)
        {
            shotSpawns[i].rotation = Quaternion.Euler(0.0f, (leftSide + degreeChange), 0.0f);
            Instantiate(shot, shotSpawns[i].position, shotSpawns[i].rotation);
        }
        for (int j = 0; j < shotSpawns2.Length; j++)
        {
            shotSpawns2[j].rotation = Quaternion.Euler(0.0f, (rightSide - degreeChange), 0.0f);
            Instantiate(shot, shotSpawns2[j].position, shotSpawns2[j].rotation);
        }
    }
    void Fire2()
    {
        for (int i = 0; i < shotSpawns3.Length; i++)
        {
            shotSpawns3[i].rotation = Quaternion.Euler(0.0f, verticalShot, 0.0f);
            Instantiate(Secondshot, shotSpawns3[i].position, shotSpawns3[i].rotation);
        }
    }
    void Fire3()
    {
        int adjustment = -20;
        for (int i = 0; i < 4; i++)
        {
            OrbSpawns[0].rotation = Quaternion.Euler(0.0f, verticalShot + adjustment, 0.0f);
            Instantiate(Orbshot, OrbSpawns[0].position, OrbSpawns[0].rotation);
            OrbSpawns[3].rotation = Quaternion.Euler(0.0f, verticalShot - adjustment, 0.0f);
            Instantiate(Orbshot, OrbSpawns[3].position, OrbSpawns[3].rotation);
            adjustment += 20;
        }
    }
    void Fire4()
    {
        int adjustment = -30;
        for (int i = 0; i < 4; i++)
        {
            OrbSpawns[1].rotation = Quaternion.Euler(0.0f, verticalShot + adjustment, 0.0f);
            Instantiate(Orbshot, OrbSpawns[1].position, OrbSpawns[1].rotation);
            OrbSpawns[2].rotation = Quaternion.Euler(0.0f, verticalShot - adjustment, 0.0f);
            Instantiate(Orbshot, OrbSpawns[2].position, OrbSpawns[2].rotation);
            adjustment += 20;
        }
    }

    IEnumerator fire()
    {
        if (Time.time > waitTimer)
        {
            waitTimer = Time.time + fireRate;
            Fire();
            source = GetComponent<AudioSource>();
            volume = Random.Range(volLow, volHigh);
            sfx = sounds[1];
            source.clip = sfx;
            source.PlayOneShot(source.clip, volume);
            degreeChange += 5;
            shots -= 1;
        }
        yield return new WaitForSeconds(fireRate);       
    }
    IEnumerator pause()
    {
        shots2 = 5;
        degreeChange = 0;
        yield return new WaitForSeconds(delay);
        Reheat2 = false;
    }
    IEnumerator fire2()
    {
        if (Time.time > waitTimer2)
        {
            waitTimer2 = Time.time + fireRate;
            Fire2();
            source = GetComponent<AudioSource>();
            volume = Random.Range(volLow, volHigh);
            sfx = sounds[1];
            source.clip = sfx;
            source.PlayOneShot(source.clip, volume);
            shots2 -= 1;
        }
        yield return new WaitForSeconds(fireRate);
    }
    IEnumerator pause2()
    {
        shots = 8;
        yield return new WaitForSeconds(delay);
        stage1Count -= 1;
        if (stage1Count <= 0)
        {
            if (GameController.bossHealth <= ((GameController.bossHealthMax / 4) * 3) && stage2Active == false)
            {
                stage2Active = true;
                stage3Active = false;
                phase3 = false;
                phase2 = true;
                phase1 = false;
                stage1Count = 4;
            }
        }
        Reheat = false;
    }
    IEnumerator fire3()
    {
        if (Time.time > waitTimer3)
        {
            waitTimer3 = Time.time + fireRate;
            if (orbs <= 8)
            {
                yield return new WaitForSeconds(0.2f);
                Fire4();
            }
            else
            {
                Fire3();
            }
            source = GetComponent<AudioSource>();
            volume = Random.Range(volLow, volHigh);
            sfx = sounds[0];
            source.clip = sfx;
            source.PlayOneShot(source.clip, volume);
            orbs -= 1;
        }
        yield return new WaitForSeconds(fireRate);
    }
    IEnumerator pause3()
    {
        stage2Count -= 1;
        if (stage2Count <= 0)
        {          
            stage2Active = false;
            stage3Active = true;

            if(GameController.bossHealth <= (GameController.bossHealthMax / 2))
                phase3 = true;

            phase2 = false;
            phase1 = true;
            stage2Count = 2;
        }
        orbs = 16;
        yield return new WaitForSeconds(delay);
        Reheat3 = false;
    }
    IEnumerator WeaponsHeated()
    {
        yield return new WaitForSeconds(6);
        phase1 = true;
    }
}
