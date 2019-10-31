using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBossLevel1 : MonoBehaviour {

    // Use this for initialization
    private int speed;
	void Start ()
    {
        speed = 1;
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        StartCoroutine(FreezeSpeed());
	}
    IEnumerator FreezeSpeed()
    {
        yield return new WaitForSeconds(4);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
