using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1WeaponController : MonoBehaviour {

    [SerializeField]
    private GameObject leftArm;
    [SerializeField]
    private GameObject rightArm;
    [SerializeField]
    private GameObject basicSpreadShot;
    [SerializeField]
    private Transform basicShotSpawn;
    [SerializeField]
    private float speed = 15;
    private bool leftReheat = true;
    private bool rightReheat = true;
    private bool phase1 = false;
    private bool phase2 = false;
    private bool phase3 = false;
    public static bool stage2Active = false;
    public static bool stage3Active = false;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(WeaponsHeated());
        StartCoroutine(ReheatLeft());
	}
	
	// Update is called once per frame
	void Update ()
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
            
        }
        if (GameController.bossHealth <= ((GameController.bossHealthMax / 4) * 3) && stage2Active == false)
        {
            stage2Active = true;
            phase2 = true;
            phase1 = false;
        }
        if (GameController.bossHealth <= (GameController.bossHealthMax / 2) && stage3Active == false)
        {
            stage2Active = false;
            stage3Active = true;
            phase3 = true;
            phase2 = true;
            phase1 = true;
        }
    }
    void Phase1()
    {
        if (leftReheat == false)
            thrustLeft();
        else if (rightReheat == false)
            thrustRight();
    }
    void Phase2()
    {

    }
    void Fire()
    {

    }
    IEnumerator thrustLeft()
    {
        StartCoroutine(ReheatRight());
        leftArm.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        yield return new WaitForSeconds(0.75f);
        leftArm.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        leftArm.GetComponent<Rigidbody>().velocity = -transform.forward * (speed / 3);
        yield return new WaitForSeconds(2.25f);
        leftArm.GetComponent<Rigidbody>().velocity = Vector3.zero;
        leftReheat = true;
    }
    IEnumerator ReheatLeft()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(thrustLeft());
        leftReheat = false;
    }
    IEnumerator thrustRight()
    {
        StartCoroutine(ReheatLeft());
        rightArm.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        yield return new WaitForSeconds(0.75f);
        rightArm.GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        rightArm.GetComponent<Rigidbody>().velocity = -transform.forward * (speed / 3);
        yield return new WaitForSeconds(2.25f);
        rightArm.GetComponent<Rigidbody>().velocity = Vector3.zero;
        rightReheat = true;
    }
    IEnumerator ReheatRight()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(thrustRight());
        rightReheat = false;
    }
    IEnumerator WeaponsHeated()
    {
        yield return new WaitForSeconds(2);
        phase1 = true;
    }
}
