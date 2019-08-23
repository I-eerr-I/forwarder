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
    protected float start_position;
    protected float distance_to_run;
    protected bool  started_dying;
    protected bool  is_dying;
    protected float dead_start_position;

    protected GameObject player;
    protected PlayerController pc;
    protected Rigidbody player_rb;

    protected Rigidbody rb;

    void Start()
    {
        player = GetComponent<AppearanceChars>().GetPlayer();
        rb = GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayerController>();
        player_rb = player.GetComponent<Rigidbody>();
        maxRunDistance = player.transform.position.z - minRunDistance;
        start_position = transform.position.z;
        distance_to_run = Random.Range(player.transform.position.z, maxRunDistance);
        started_dying = false;
        is_dying      = false;
    }

    void Update()
    {
        if(player.activeInHierarchy)
        {
            if(distance_to_run <= 0)
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF DISTANCE <= 0!");
                Destroy(gameObject);
            }
            if(start_position - distance_to_run < transform.position.z)
            {
                Action();        
            }
            else
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF RUNNING TOO MUCH!");
                started_dying = true;
            }
        }
        else
        {
            light.intensity *= 2;
            started_dying = true;
        }
        if(started_dying)
        {
            dead_start_position = transform.position.z;
            is_dying = true;
            started_dying = false;            
        }
        if(is_dying)
            Death();
    }

    protected abstract void Action();

    protected virtual void Death()
    {
        if(start_position + deadRunDistance > transform.position.z)
            rb.MovePosition(transform.position + (transform.forward * (speed*2f) * Time.deltaTime));
        else
            Destroy(gameObject);
    }

     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            player_rb.AddForce(new Vector3(0,0, -power * Time.deltaTime), ForceMode.Impulse);
            Debug.Log(gameObject.name + ": DEAD BECAUSE OF HIT!");
            Death();
        }
    }
}
