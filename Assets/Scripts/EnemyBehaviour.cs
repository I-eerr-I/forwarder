using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    public Light light;

    protected bool  startedDying;
    protected bool  isDying;
    protected float deadStartPosition;
    protected bool isAction;

    protected GameObject player;
    protected PlayerController pc;
    protected Rigidbody playerRb;

    protected EnemyParameters parameters;
    protected Rigidbody rb;
    protected BoxCollider boxCollider;
    protected AudioSource audioSource;

    void Start()
    {
        parameters       = GetComponent<EnemyParameters>();
        player           = parameters.GetPlayer();
        playerRb         = player.GetComponent<Rigidbody>(); 
        pc               = player.GetComponent<PlayerController>();
        rb               = GetComponent<Rigidbody>();
        boxCollider      = GetComponent<BoxCollider>();
        audioSource      = GetComponent<AudioSource>();
        startedDying     = false;
        isDying          = false;
        isAction         = false;
        audioSource.clip = parameters.idlingSound;
    }

    void Update()
    {
        if(player.activeInHierarchy)
        {
            if(parameters.hp <= 0 && !isDying)
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
        if(isAction && audioSource.clip != parameters.actionSound)
        {
            audioSource.clip = parameters.actionSound;
            audioSource.volume += 10f;
            audioSource.Play();
        }
    }

    protected abstract void Action();

    protected virtual void Death()
    {
        if(deadStartPosition + parameters.deadRunDistance > transform.position.z)
        {
            rb.MovePosition(transform.position + (transform.forward * (parameters.speed*2f) * Time.deltaTime));
        }
            
        else
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            playerRb.AddForce(new Vector3(0,0, -parameters.power * Time.deltaTime));
            StartCoroutine("TurnOffCollider");
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Light Power Field"))
        {
            parameters.hp -= pc.lightPower * Time.deltaTime;
        }
    }

    IEnumerator TurnOffCollider()
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(1f);
    }
}
