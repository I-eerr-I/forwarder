using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManager gameManager;
    public Animator doorAnimator;
    public AudioSource officerDoorAudio;

    void Start()
    {
        doorAnimator.enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            doorAnimator.enabled = true;
            officerDoorAudio.Play();
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
