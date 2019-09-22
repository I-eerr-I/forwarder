using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSoundController : MonoBehaviour
{
    [Header("Sounds")]
    public AudioClip lightOn;
    public AudioClip powerLightOn;

    [Header("Parameters")]
    public float powerLightOnVolume = 0.8f;

    AudioSource audioSource;
    Animator    lightAnimator;

    void Update()
    {
        if(!lightAnimator.GetBool("Power Light Still On") && audioSource.isPlaying && audioSource.clip == powerLightOn && !lightAnimator.GetBool("Power Light Countdown"))
        {
            audioSource.Stop();
        }
    }

    void Awake()
    {
        lightAnimator = GetComponent<Animator>();
        audioSource   = GetComponent<AudioSource>();
    }

    public void LightOnSound()
    {
        audioSource.volume = 1f;
        audioSource.clip = lightOn;
        audioSource.Play();
    }

    public void PowerLightOnSound()
    {
        audioSource.volume = powerLightOnVolume;
        audioSource.clip = powerLightOn;
        audioSource.Play();
    }
}
