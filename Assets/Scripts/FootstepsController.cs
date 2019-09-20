using UnityEngine;
using Random = UnityEngine.Random;

public class FootstepsController : MonoBehaviour
{   
    public AudioClip[] footsteps;
    
    float speedDelayCoef = 0.05f;
    float speedPitchCoef = 0.5f;
    float delay;

    AudioSource      audioSource;
    PlayerController playerController;

    void Start()
    {
        audioSource      = GetComponent<AudioSource>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        delay = speedDelayCoef/playerController.speed;
        audioSource.pitch = playerController.speed/speedPitchCoef; 
        if(Input.GetButton("Vertical") && !audioSource.isPlaying)
        {
            audioSource.clip = footsteps[Random.Range(0, footsteps.Length-1)];
            audioSource.PlayDelayed(delay);
        }
    }
}
