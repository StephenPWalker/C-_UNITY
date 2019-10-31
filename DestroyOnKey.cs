using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnKey : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(Delete());   
    }
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.1f);
        if (Input.GetAxis("P1_Horizontal") > 0.5 || Input.GetAxis("P1_Horizontal") < -0.5 || Input.GetButton("P1_Fire1"))
        {
            Destroy(gameObject);
        }
    }
}