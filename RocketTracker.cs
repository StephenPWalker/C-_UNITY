using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTracker : MonoBehaviour {

    public float speed = 3.0f;
    public GameObject m_Target = null;
    Vector3 m_lastKnownPos = Vector3.zero;
    Quaternion m_lookAtRotation;
    // Update is called once per frame
    void Update()
    {
        if (m_Target == null)
        {
            m_Target = GameObject.FindGameObjectWithTag("Enemy");
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, speed * Time.deltaTime);
            }
        }
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
