using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private string direction;
    [SerializeField]
    private int speed;
    private GameObject m_Target;
    private string targetName;
    private bool isMoving;
    Vector3 m_lastKnownPos = Vector3.zero;
    Quaternion m_lookAtRotation;
    private Rigidbody rb;
    // Update is called once per frame
    private void Start()
    {
        isMoving = true;
        if (direction == "Down")
        {
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == "Up")
        {
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction == "Left")
        {
            this.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction == "Right")
        {
            this.transform.localRotation = Quaternion.Euler(0, 270, 0);
        }
        StartCoroutine(MovingFreeze());
    }
    void Update ()
    {
        if (m_Target == null && transform.root.tag == "Enemy")
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

        if (m_Target)
        {
            if (m_lastKnownPos != m_Target.transform.position)
            {
                m_lastKnownPos = m_Target.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(-m_lastKnownPos - transform.position);
            }

            if (transform.rotation != m_lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, (speed * 2) * Time.deltaTime);
            }
        }
        if (isMoving)
            GetComponent<Rigidbody>().velocity = -transform.forward * speed;
            //transform.Translate((-Vector3.forward * speed) * Time.deltaTime);
    }

    public void SetDirection(string direction)
    {
        this.direction = direction;
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
    bool SetTarget(GameObject target)
    {
        if (target)
        {
            return false;
        }

        m_Target = target;

        return true;
    }
    IEnumerator MovingFreeze()
    {
        yield return new WaitForSeconds(5);
        isMoving = false;
    }
}
