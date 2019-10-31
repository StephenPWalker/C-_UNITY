using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuverV2 : MonoBehaviour
{
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Vector2 startWait;

    public Rigidbody rb;
    public Boundary boundary;

    private float currentSpeed;
    private float targetManeuver;

    public float tilt;
    public float dodge;
    public float smoothing;

    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject playerTransformObject = GameObject.FindGameObjectWithTag("Player");
        if (playerTransformObject != null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            targetManeuver = Random.Range(1, dodge * -Mathf.Sign(transform.position.x));
        }
        currentSpeed = rb.velocity.z;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            GameObject playerTransformObject = GameObject.FindGameObjectWithTag("Player");
            if (playerTransformObject != null)
            {
                targetManeuver = playerTransform.position.x;
            }
            else
            {
                targetManeuver = Random.Range(1, dodge * -Mathf.Sign(transform.position.x));
            }
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
        rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
