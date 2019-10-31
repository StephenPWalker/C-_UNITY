using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreMover : MonoBehaviour {

    public Vector3 direction;
    private float speed = 200;
    private bool done = false;
	// Update is called once per frame
	void Update ()
    {
        if (GameController.isLevelOver == true && done == false)
        {
            done = true;
            StartCoroutine(move());
        }
	}
    IEnumerator move()
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
        yield return new WaitForSeconds(2.5f);
        GetComponent<Rigidbody>().isKinematic = true;
        speed = 0;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
