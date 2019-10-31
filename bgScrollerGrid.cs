using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgScrollerGrid : MonoBehaviour
{
    private float scrollSpeed = 3f;
    [SerializeField]
    private float length;
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, length);
        transform.position = startPos + Vector3.back * newPos;
    }

}
