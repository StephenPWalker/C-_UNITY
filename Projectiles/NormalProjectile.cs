using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : BasicProjectile {

    Vector3 myPos;
    Vector3 m_direction;
    bool m_fired;
    // Update is called once per frame
    private void Start()
    {
        myPos = transform.position;
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(myPos.x, 0, myPos.z);
        }
    }

    void Update ()
    {
        if (m_fired)
        {
            transform.position += m_direction * (speed * Time.deltaTime);
        }
	}

    public override void FireProjectile(GameObject launcher, GameObject target, int damage)
    {
        if (launcher && target)
        { 
                m_direction = (target.transform.position - launcher.transform.position).normalized;
                m_fired = true;
        }
    }
}
