using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover_R2L : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private float currentSpeed;
    public float tilt;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        currentSpeed = rb.velocity.z;
        rb.velocity = new Vector3(-currentSpeed, 0.0f, -currentSpeed);
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
