using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : BasicProjectile
{
    public GameObject m_target;
    float selfDestruct = 3.5f;
    public GameObject explode;
    private Rigidbody rb;
    private string targetName;
    public float rotateSpeed;
    private int moveSpeed = 10;
    private float forwardTime = .5f;

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {       
        selfDestruct -= Time.deltaTime;
        if (selfDestruct <= 0)
        {
            Instantiate(explode, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        forwardTime -= Time.deltaTime;
        if (m_target == null)
        {
            m_target = FindClosestEnemy();
        }
        if (targetName == "Boundary")
        {
            m_target = FindClosestEnemy();
        }
    }
    void FixedUpdate()
    {
        if (m_target == null)
        {
            m_target = FindClosestEnemy();
        }
        if (targetName == "Boundary")
        {
            m_target = FindClosestEnemy();
        }
        if (forwardTime > 0)
        {
            rb.velocity = transform.right * -moveSpeed;
        }
        else
        {
            Vector3 direction = m_target.transform.position - rb.position;
            direction.Normalize();
            Vector3 rotateAmount = Vector3.Cross(transform.right / 5, direction);
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.right * -moveSpeed;
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            targetName = "Enemy";
            gos = GameObject.FindGameObjectsWithTag("Enemy");
        }
        else if (GameObject.FindGameObjectWithTag("Boss"))
        {
            targetName = "Boss";
            gos = GameObject.FindGameObjectsWithTag("Boss");
        }
        else
        {
            targetName = "Boundary";
            gos = GameObject.FindGameObjectsWithTag("Boundary");
        }

        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public override void FireProjectile(GameObject launcher, GameObject target, int damage)
    {
        if (target)
        {
            m_target = target;
        }
    }
}
