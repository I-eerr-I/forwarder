using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    
    
    public float speed;
    public float power;
    public float minRunDistance;

    protected float maxRunDistance;
    protected float start_position;
    protected float distance_to_run;
    protected bool  updated_chars;

    protected GameObject player;
    protected PlayerController pc;
    protected Rigidbody player_rb;

    protected Rigidbody rb;

    void Start()
    {      
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        player_rb = player.GetComponent<Rigidbody>();
        if(player.activeInHierarchy)
        {
            if(!updated_chars)
            {
                maxRunDistance = player.transform.position.z - minRunDistance;
                start_position = transform.position.z;
                distance_to_run = Random.Range(player.transform.position.z, maxRunDistance);
                updated_chars   = true;
            }
            if(distance_to_run <= 0)
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF DISTANCE <= 0!");
                Death();
            }
            if(start_position - distance_to_run < transform.position.z)
            {
                Action();        
            }
            else
            {
                Debug.Log(gameObject.name + ": DEAD BECAUSE OF RUNNING TOO MUCH!");
                Death();
            }
        }
    }

    protected abstract void Action();

    protected virtual void Death()
    {
        updated_chars = false;
        Destroy(gameObject);
    }

     void OnCollisionEnter(Collision col)
    {
        if(gameObject.activeInHierarchy && col.gameObject.tag == "Player")
        {
            player_rb.AddForce(new Vector3(0,0, -power), ForceMode.Impulse);
            Debug.Log(gameObject.name + ": DEAD BECAUSE OF HIT!");
            Death();
        }
    }
}
