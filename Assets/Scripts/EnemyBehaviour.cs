using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    
    
    public float speed;
    public float power;
    public float minRunDistance;
    public float deadRunDistance;

    public Light light;

    protected float maxRunDistance;
    protected float startPosition;
    protected float distanceToRun;
    protected bool  startedDying;
    protected bool  isDying;
    protected float deadStartPosition;

    protected GameObject player;
    protected PlayerController pc;
    protected Rigidbody playerRb;

    protected Rigidbody rb;

    void Start()
    {
        player   = GetComponent<AppearanceChars>().GetPlayer();
        playerRb = player.GetComponent<Rigidbody>(); 
        pc       = player.GetComponent<PlayerController>();
        rb       = GetComponent<Rigidbody>();
        maxRunDistance  = player.transform.position.z - minRunDistance;
        startPosition   = transform.position.z;
        distanceToRun   = Random.Range(player.transform.position.z, maxRunDistance);
        startedDying    = false;
        isDying         = false;
    }

    void Update()
    {
        if(player.activeInHierarchy)
        {
            if(distanceToRun <= 0)
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF DISTANCE <= 0!");
                Destroy(gameObject);
            }
            if(startPosition - distanceToRun < transform.position.z)
            {
                Action();        
            }
            else
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF RUNNING TOO MUCH!");
                startedDying = true;
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
    }

    protected abstract void Action();

    protected virtual void Death()
    {
        if(startPosition + deadRunDistance > transform.position.z)
            rb.MovePosition(transform.position + (transform.forward * (speed*2f) * Time.deltaTime));
        else
            Destroy(gameObject);
    }

     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            playerRb.AddForce(new Vector3(0,0, -power * Time.deltaTime), ForceMode.Impulse);
            Debug.Log(gameObject.name + ": DEAD BECAUSE OF HIT!");
            Death();
        }
    }
}
