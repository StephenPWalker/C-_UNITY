using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float tilt;
    public float smoothing;
    private string whichPlayer;
    public Boundary boundary;
    public GameObject[] shots;
    public Transform[] shotSpawns;
    public Transform[] missileSpawn;

    public int endNumberOfGuns;
    public int startNumberOfGuns;

    // will have to rethink how this is done for multtple porjectiles and weapons
    public static bool isShieldOn = false;
    public static bool isShield2On = false;
    public static bool isGamePaused = false;

    public int m_playerDamage;
    public int m_playerDamage2;
    public int m_playerDamage3;
    public int m_playerDamage4;

    private int playerDamage; // always reserved for main gun
    public string secondaryWeapon;
    public string utilityAbility;
    private int playerDamage2; // secondary weapon
    private int playerDamage3; // ability
    private int playerDamage4; // special

    private float nextFire;
    private float nextFire2;
    private float nextFire3;
    public float mFireRate;
    public float mFireRate2;
    public float mFireRate3;
    private float fireRate;
    private float fireRate2;
    private float fireRate3;
    public int numberOfWeapons;
    private string playerPrefix;

    private void Start()
    {
        GameController.Counter += 1;
        whichPlayer = "Player" + GameController.Counter;
        playerDamage = m_playerDamage;
        playerDamage2 = m_playerDamage2;
        playerDamage3 = m_playerDamage3;
        playerDamage4 = m_playerDamage4;
        fireRate = mFireRate;
        fireRate2 = mFireRate2;
        fireRate3 = mFireRate3;
        if (whichPlayer == "Player1")
        {
            gameObject.tag = "Player";
            playerPrefix = "P1_";
        }
        if (whichPlayer == "Player2")
        {
            gameObject.tag = "Player2";
            playerPrefix = "P2_";
        }
    }
    private void Update()
    {
        if (GameController.isLevelOver != true)
        {
            if (!isGamePaused)
            {
                if(Input.GetButtonUp(playerPrefix + "Fire3") && Time.time > nextFire3)
                {
                    if (utilityAbility.Equals("Shield"))
                    {
                        Shield();
                    }
                }

                if (numberOfWeapons > 0)
                {
                    if (Input.GetButton(playerPrefix + "Fire1") && Time.time > nextFire)
                    {
                        nextFire = Time.time + fireRate;
                        for (int shotSpawn = startNumberOfGuns; shotSpawn < endNumberOfGuns; shotSpawn++)
                        {
                            Instantiate(shots[0], shotSpawns[shotSpawn].position, shotSpawns[shotSpawn].rotation);
                            TagItLaser();
                        }
                        //playerDamage = 5 * (endNumberOfGuns - startNumberOfGuns); this only makes damage multiply exponentially
                        //GetComponent<AudioSource>().Play();
                    }
                    if (numberOfWeapons > 1)
                    {
                        if (Input.GetButton(playerPrefix + "Fire2") && Time.time > nextFire2)
                        {
                            nextFire2 = Time.time + fireRate2;
                            if (secondaryWeapon == "Missiles")
                            {
                                StartCoroutine(Missiles());
                            }
                            if (secondaryWeapon == "Spread shot")
                            {
                                StartCoroutine(SpreadShot());
                            }
                        }
                    }
                }
            }
        }
    }
    void Shield()
    {
        if(whichPlayer == "Player1")
        {
            if (isShieldOn == false)
            {
                nextFire3 = Time.time + fireRate3;
                isShieldOn = true;
            }
            else if (isShieldOn)
            {
                nextFire3 = Time.time + fireRate3;
                isShieldOn = false;
            }
        }
        else
        {
            if (isShield2On == false)
            {
                nextFire3 = Time.time + fireRate3;
                isShield2On = true;
            }
            else if (isShield2On)
            {
                nextFire3 = Time.time + fireRate3;
                isShield2On = false;
            }
        }
    }
    void TagItLaser()
    {
        if (whichPlayer == "Player2")
        {
            shots[0].tag = "Laser2";
        }
        else
        {
            shots[0].tag = "Laser";
        }
    }
    void TagItProj()
    {
        if (whichPlayer == "Player2")
        {
            shots[0].tag = "Projectile2";
        }
        else
        {
            shots[0].tag = "Projectile";
        }
    }
    private void FixedUpdate()
    {
        if (GameController.isLevelOver != true)
        {
            float moveHorizontal = Input.GetAxis(playerPrefix + "Horizontal");
            float moveVertical = Input.GetAxis(playerPrefix + "Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;
            GetComponent<Rigidbody>().position = new Vector3
                (
                    Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
                );
                Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
                GetComponent<Rigidbody>().rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
        }
        if (GameController.isLevelOver == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            speed = 0;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    IEnumerator SpreadShot()
    {
        for (int i = 0; i < missileSpawn.Length; i++)
        {
            Instantiate(shots[1], missileSpawn[i].position, missileSpawn[i].rotation);
            TagItLaser();
        }
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(shots[1], missileSpawn[i].position, missileSpawn[i].rotation);
            TagItLaser();
        }
        yield return new WaitForSeconds(0.1f);
        Instantiate(shots[1], missileSpawn[0].position, missileSpawn[0].rotation);
        yield return null;
    }
    IEnumerator Missiles()
    {
        Instantiate(shots[1], missileSpawn[0].position, missileSpawn[0].rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(shots[1], missileSpawn[0].position, missileSpawn[0].rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(shots[1], missileSpawn[0].position, missileSpawn[0].rotation);
    }
}
