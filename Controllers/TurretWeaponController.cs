using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeaponController : MonoBehaviour {

    public float fireRate;
    public int shotsFired;
    public int damage;
    public float fieldOfView;
    public bool beam;
    private string targetName;
    public List<GameObject> projectileSpawns;
    public GameObject projectile;
    public GameObject target;
    [SerializeField]
    private float shotDelay = 0.1f;
    [SerializeField]
    private bool isInactive;
    [SerializeField]
    float m_fireTimer = 0.0f;
    List<GameObject> m_lastProjectiles = new List<GameObject>();

    private void Start()
    {
        if (shotsFired <= 0)
        {
            shotsFired = 1;
        }
        m_fireTimer = 3;
    }
    private void Update()
    {
        if (target == null && transform.root.tag == "Enemy" || transform.root.tag == "Boss")
        {
            int targetting = Random.Range(1, 100);
            if (targetting <= 50)
            {
                target = GameObject.FindGameObjectWithTag("Player");
                if (target == null)
                {
                    target = GameObject.FindGameObjectWithTag("Player2");
                }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player2");
                if (target == null)
                {
                    target = GameObject.FindGameObjectWithTag("Player");
                }
            }
 
        }
        else if (target == null && (transform.root.tag == "Player" || transform.root.tag == "Player2"))
        {
            target = FindClosestEnemy();
        }

        if (targetName == "Boundary")
        {
            target = FindClosestEnemy();
        }

        if (!isInactive)
        {
            if (target != null)
            {
                if (beam && m_lastProjectiles.Count <= 0)
                {
                    float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position));
                    if (angle < fieldOfView)
                    {
                        SpawnProjectiles();
                    }
                }
                else if (beam && m_lastProjectiles.Count > 0)
                {
                    float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position));

                    if (angle > fieldOfView)
                    {
                        while (m_lastProjectiles.Count > 0)
                        {
                            Destroy(m_lastProjectiles[0]);
                            m_lastProjectiles.RemoveAt(0);
                        }
                    }
                }
                else
                {
                    m_fireTimer += Time.deltaTime;

                    if (m_fireTimer >= fireRate)
                    {
                        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position));
                        if (angle < fieldOfView)
                        {
                            StartCoroutine(shots(angle));
                            m_fireTimer = 0.0f;
                        } 
                    }
                }
            }
        }
    }
    public bool getInactive()
    {
        return isInactive;
    }
    public void setActive()
    {
        isInactive = false;
    }
    public void setInactive()
    {
        isInactive = true;
    }
    IEnumerator shots(float angle)
    {
        for (int i = 0; i < shotsFired; i++)
        {
            if (target && angle < fieldOfView)
                SpawnProjectiles();
            yield return new WaitForSeconds(shotDelay);
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
    void SpawnProjectiles()
    {
        if (!projectile || targetName == "Boundary" || target == null)
        {
            return;
        }
        for (int i = 0; i < projectileSpawns.Count; i++)
        {
            if (projectileSpawns[i])
            {
                GameObject proj = Instantiate(projectile, projectileSpawns[i].transform.position, projectileSpawns[i].transform.rotation);
                proj.GetComponent<BasicProjectile>().FireProjectile(projectileSpawns[i], target, damage);
                m_lastProjectiles.Add(proj);
            }
        }
    }

}
