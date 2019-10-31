using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotationController : MonoBehaviour {

    [SerializeField]
    private float speed = 60f;
    [SerializeField]
    private GameObject m_Target = null;
    Vector3 m_lastKnownPos = Vector3.zero;
    Quaternion m_lookAtRotation;

    public bool limitTraverse = false;
    [Range(0.0f, 90.0f)]
    public float elevation = 60.0f;
    [Range(0.0f, 90.0f)]
    public float depression = 5.0f;
    // Update is called once per frame
    void Update()
    {
        if (m_Target == null)
        m_Target = GetComponentInParent<TurretTracker>().GetTarget();

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
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, GetComponentInParent<Transform>().eulerAngles.y, 0);
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
