using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour {

    private Vector3 myPos;
    public Vector3 start;
    public Vector3 mid;
    public Vector3 end;
    private Vector3 target;
    private Rigidbody rb;
    private Quaternion lookAt;
    public float speed;
    public float rotateSpeed;
    private Vector3 nextTarget;
    private string whichTarget;
    private int counter = 0;
    public float tilt;
    Vector3 rotationAngle = Vector3.zero;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        myPos = transform.position;
        target = start;
        whichTarget = "Start";        
    }

    // Update is called once per frame
    void Update()
    {
        if (whichTarget == "Start")
        {
            nextTarget = mid;
        }
        else if (whichTarget == "Mid")
        {
            nextTarget = end;
        }
        myPos = transform.position;

        if (Vector3.Distance(myPos, target) < 1)
        {
            target = nextTarget;
            counter += 1;
            if (counter == 1)
            {
                whichTarget = "Mid";
            }
            else if (counter == 2)
            {
                whichTarget = "End";
            }
        } 

        Vector3 direction = target - rb.position;
        direction.Normalize();
        Vector3 rotateAmount = Vector3.Cross(transform.forward / 5, direction);
        rb.angularVelocity = rotateAmount * -rotateSpeed;
        rb.velocity = transform.forward * -speed;

        Vector3 theDirection = Quaternion.FromToRotation(transform.rotation * Vector3.up, Quaternion.identity * Vector3.up) * rb.velocity;
        theDirection = Quaternion.Inverse(transform.rotation) * rb.velocity;
        
        if (theDirection.x < 0)
        {
            rotationAngle += transform.transform.forward * -tilt;
        }
        else if(theDirection.x > 0)
        {
            rotationAngle += transform.transform.forward * tilt;
        }
        transform.RotateAround(transform.localPosition, rotationAngle, speed * Time.deltaTime);
        
    }
    public void setStart(Vector3 startPos)
    {
        start = startPos;
    }
    public void setMid(Vector3 midPos)
    {
        mid = midPos;
    }
    public void setEnd(Vector3 endPos)
    {
        end = endPos;
    }
}
