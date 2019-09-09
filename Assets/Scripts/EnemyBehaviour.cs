using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    
    
    public float speed;
    public float power;
    public float deadRunDistance;
    public float hp = 5f;
    public AudioClip idlingSound;
    public AudioClip actionSound;

    public Light light;

    protected bool  startedDying;
    protected bool  isDying;
    protected float deadStartPosition;
    protected bool isAction;

    protected GameObject player;
    protected PlayerController pc;
    protected Rigidbody playerRb;

    protected Rigidbody rb;
    protected BoxCollider boxCollider;
    protected AudioSource audioSource;

    void Start()
    {
        player           = GetComponent<AppearanceChars>().GetPlayer();
        playerRb         = player.GetComponent<Rigidbody>(); 
        pc               = player.GetComponent<PlayerController>();
        rb               = GetComponent<Rigidbody>();
        boxCollider      = GetComponent<BoxCollider>();
        audioSource      = GetComponent<AudioSource>();
        startedDying     = false;
        isDying          = false;
        isAction         = false;
        audioSource.clip = idlingSound;
    }

    void Update()
    {
        if(player.activeInHierarchy)
        {
            if(hp <= 0 && !isDying)
            {
                startedDying = true;
            }
            else
            {
                Action();        
            }
        }
        else
        {
            light.intensity *= 2;
            startedDying = true;
        }
        if(startedDying)
        {
            deadStartPosition = transform.position.z;
            isDying = true;
            startedDying = false;            
        }
        if(isDying)
            Death();
        if(isAction && audioSource.clip != actionSound)
        {
            audioSource.clip = actionSound;
            audioSource.volume += 10f;
            audioSource.Play();
        }
    }

    protected abstract void Action();

    protected virtual void Death()
    {
        if(deadStartPosition + deadRunDistance > transform.position.z)
        {
            rb.MovePosition(transform.position + (transform.forward * (speed*2f) * Time.deltaTime));
        }
            
        else
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            playerRb.AddForce(new Vector3(0,0, -power * Time.deltaTime));
            StartCoroutine("TurnOffCollider");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Light Power Field"))
        {
            hp -= pc.lightPower * Time.deltaTime;
        }
    }

    IEnumerator TurnOffCollider()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(1f);
    }
}
