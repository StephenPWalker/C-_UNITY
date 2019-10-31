using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManeuver : MonoBehaviour
{
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Vector2 startWait;

    public Rigidbody rb;
    public Boundary boundary;

    public static float currentSpeed;
    public static float targetManeuver;
    public static float Speed;
    public float tilt;
    public float dodge;
    public float smoothing;
    public bool wasActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = rb.velocity.z;
        Speed = 4;
        StartCoroutine(Go());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (WeaponControllerBoss.stage2Active == false)
        {
                targetManeuver = Random.Range(1, dodge * -Mathf.Sign(transform.position.x));
                yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
                targetManeuver = 0;
                yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }
    IEnumerator Go()
    {
        yield return new WaitForSeconds(6);
        currentSpeed = 0.0f;
        StartCoroutine(Evade());
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
        if (WeaponControllerBoss.stage2Active == true)
        {
            wasActive = true;
        }
        if (wasActive == true && WeaponControllerBoss.stage2Active == false)
        {
            StartCoroutine(Evade());
            wasActive = false;
        }
    }
}
