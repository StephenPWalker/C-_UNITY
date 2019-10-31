using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTracker : MonoBehaviour {

    [SerializeField]
    private bool isTurret = false;
    [SerializeField]
    private bool isStationaryTurret = false;
    [SerializeField]
    private Transform Body;
    [SerializeField]
    private Transform[] Turrets;
    [SerializeField]
    private float speed = 60f;
    [SerializeField]
    private GameObject m_Target = null;
    private string targetName;
    Vector3 m_lastKnownPos = Vector3.zero;
    Quaternion m_lookAtRotation;

    public bool limitTraverse = false;
    [Range(0.0f, 180.0f)]
    public float leftTraverse = 60.0f;
    [Range(0.0f, 180.0f)]
    public float rightTraverse = 60.0f;




    // Update is called once per frame
    void Update ()
    {
        if (m_Target == null && transform.root.tag == "Enemy" || transform.root.tag == "Boss")
        {
            int targetting = Random.Range(1, 100);
            if (targetting <= 50)
            {
                m_Target = GameObject.FindGameObjectWithTag("Player");
                if (m_Target == null)
                {
                    m_Target = GameObject.FindGameObjectWithTag("Player2");
                }
            }
            else
            {
                m_Target = GameObject.FindGameObjectWithTag("Player2");
                if (m_Target == null)
                {
                    m_Target = GameObject.FindGameObjectWithTag("Player");
                }
            }
        }
        else if (m_Target == null && (transform.root.tag == "Player" || transform.root.tag == "Player2"))
        {
            m_Target = FindClosestEnemy();
        }

        if (targetName == "Boundary")
        {
            m_Target = FindClosestEnemy();
        }

        if (m_Target)
        {
            if (m_lastKnownPos != m_Target.transform.position)
            {
                m_lastKnownPos = m_Target.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(m_lastKnownPos - transform.position);
            }

            if (transform.rotation != m_lookAtRotation)
            {
                Quaternion rot = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, speed * Time.deltaTime);
                transform.rotation = rot;
                if (isStationaryTurret)
                {
                    Body.eulerAngles = new Vector3(-90, transform.eulerAngles.y + 90, 0);
                    for (int i = 0; i < Turrets.Length; i++)
                        Turrets[i].eulerAngles = new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z-90);
                }
                else
                {
                    if (isTurret)
                    {
                        Body.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        for (int i = 0; i < Turrets.Length; i++)
                            Turrets[i].eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                    }
                }
            }
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
    public GameObject GetTarget()
    {
        return m_Target;
    }
    bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }

        m_Target = target;

        return true;
    }
}
