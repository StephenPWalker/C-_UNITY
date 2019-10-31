using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnKeyP2 : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Delete());
    }
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.1f);
        if (Input.GetAxis("P2_Horizontal") > 0.5 || Input.GetAxis("P2_Horizontal") < -0.5 || Input.GetButton("P2_Fire1"))
        {
            Destroy(gameObject);
        }
    }
}