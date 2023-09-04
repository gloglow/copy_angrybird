using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject[] Sounds;
    AudioSource audioSource;

    public void damagedSound()
    {
        audioSource = Sounds[0].GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void destSound()
    {
        audioSource = Sounds[1].GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void bowChargeSound()
    {
        audioSource = Sounds[2].GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void ShootingSound()
    {
        audioSource = Sounds[3].GetComponent<AudioSource>();
        audioSource.Play();
    }
}
