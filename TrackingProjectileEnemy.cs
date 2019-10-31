using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectileEnemy : BasicProjectile
{ 
    public GameObject m_target;
    float selfDestruct = 3.5f;
    public GameObject explode;
    private Rigidbody rb;
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
            int whichOne = Random.Range(1, 100);
            if (whichOne >= 50)
            {
                if (GameObject.FindGameObjectWithTag("Player2"))
                {
                    m_target = GameObject.FindGameObjectWithTag("Player2");
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player"))
                    {
                        m_target = GameObject.FindGameObjectWithTag("Player");
                    }
                }
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Player"))
                {
                    m_target = GameObject.FindGameObjectWithTag("Player");
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player2"))
                    {
                        m_target = GameObject.FindGameObjectWithTag("Player2");
                    }
                }
            }
        }
        if(m_target == null)
        {
            m_target = GameObject.FindGameObjectWithTag("Boundary");
        }        
    }
    void FixedUpdate()
    {
        if (m_target == null)
        {
            int whichOne = Random.Range(1, 100);
            if (whichOne >= 50)
            {
                if (GameObject.FindGameObjectWithTag("Player2"))
                {
                    m_target = GameObject.FindGameObjectWithTag("Player2");
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player"))
                    {
                        m_target = GameObject.FindGameObjectWithTag("Player");
                    }
                }
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Player"))
                {
                    m_target = GameObject.FindGameObjectWithTag("Player");
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Player2"))
                    {
                        m_target = GameObject.FindGameObjectWithTag("Player2");
                    }
                }
            }
        }
        if (m_target == null)
        {
            m_target = GameObject.FindGameObjectWithTag("Boundary");
        }
        if (forwardTime > 0)
        {
            rb.velocity = -transform.forward * -moveSpeed;
        }
        else
        {
            Vector3 direction = m_target.transform.position - rb.position;
            direction.Normalize();
            Vector3 rotateAmount = Vector3.Cross(-transform.forward / 5, direction);
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = -transform.forward * -moveSpeed;
        }
    }
    public override void FireProjectile(GameObject launcher, GameObject target, int damage)
    {
        if (target)
        {
            m_target = target;
        }
    }
}
