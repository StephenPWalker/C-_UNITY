using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGStroller : MonoBehaviour
{
    private float scrollSpeed = 3f;

    void Update()
    {
        transform.Translate(-Vector3.forward * (Time.deltaTime * scrollSpeed));
    }
}
