using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBoss : MonoBehaviour
{
    public float speed;
    public float stopTime = 0;
    private int done = 0;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    private void Update()
    {
        // need to check a way to make it stop then start being able to do evasicve manuvers again
        stopTime += Time.deltaTime;
        if (stopTime >= 6 && done == 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            //GetComponent<Rigidbody>().velocity = Vector3.zero;
            speed = 0;
            done = 1;
            StartCoroutine(kinematic());
        }
    }
    IEnumerator kinematic()
    {
        yield return new WaitForSeconds(4);
        GetComponent<Rigidbody>().isKinematic = false;
    }
}

