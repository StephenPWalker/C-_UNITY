using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour
{
	public float lifetime;
    public bool isTracking;

	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
    private void Update()
    {
        if (transform.position.y != 0 && !isTracking)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
