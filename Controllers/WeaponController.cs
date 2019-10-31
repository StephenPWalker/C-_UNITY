using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform[] shotSpawns;
    public float [] shotSpawnRotations;
    public float explode;
    public GameObject Explosion;
    bool hasExploded = false;

    private AudioSource source;
    public AudioClip sfx;
    private float volLow = 0.2f;
    private float volHigh = 0.5f;
    private float volume;

    private void Update()
    {       
        explode -= Time.deltaTime;
        if (explode <= 0 && hasExploded == false)
        {
            source = GetComponent<AudioSource>();
            volume = Random.Range(volLow, volHigh);
            source.PlayOneShot(sfx, volume);
            Fire();
            hasExploded = true;
        }
    }

    void Fire()
    {
        for (int i = 0; i < shotSpawns.Length; i++)
        {
            shotSpawns[i].rotation = Quaternion.Euler(0.0f, shotSpawnRotations[i], 0.0f);
            Instantiate(shot, shotSpawns[i].position, shotSpawns[i].rotation);
        }
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
