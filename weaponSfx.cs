using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSfx : MonoBehaviour {

    private AudioSource source;
    public AudioClip sfx;
    private float volLow = 0.2f;
    private float volHigh = 0.5f;
    private float volume;

    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
        volume = Random.Range(volLow, volHigh);
        source.PlayOneShot(sfx, volume);
    }
}
