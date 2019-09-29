using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public GameManager gameManager;
    public Animator    doorAnimator;
    public AudioSource doorAudio;
    public Transform   startLevelEnd;
    public Transform   startUpgrade;    
    public AudioSource startUpgradeAudio;

    bool done            = false;
    bool startedLevelEnd = false;
    bool startedUpgrade  = false;
    
    Transform player;

    void Start()
    {
        doorAnimator.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Debug.Log(startUpgradeAudio.isPlaying.ToString());
        if(player.gameObject.activeSelf && player.position.z >= startLevelEnd.position.z && !startedLevelEnd)
        {
            gameManager.EndLevel();
            startedLevelEnd = true;
        }

        if(player.gameObject.activeSelf && player.position.z >= startUpgrade.position.z && !startedUpgrade)
        {
            startUpgradeAudio.Play();
            gameManager.StartUpgrade();
            startedUpgrade = true;
        }
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
}
