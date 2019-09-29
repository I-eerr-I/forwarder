using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManager gameManager;
    public Animator doorAnimator;
    public AudioSource doorAudio;

    private bool done;

    void Start()
    {
        doorAnimator.enabled = false;
        done = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player") && !done)
        {
            doorAnimator.enabled = true;
            doorAudio.Play();
            done = true;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            gameManager.EndLevel();
        }
            
    }
}
