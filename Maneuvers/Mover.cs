using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
