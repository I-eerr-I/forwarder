using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Light light;

    public int   maxHP         = 1;
    public float speed         = 1f;
    public float lightOnTime   = 5f;
    
    private int hp;
    private float last_max_position;
    private float max_light = 3f;
    private float min_light;

    private float movement;

    private int   UPGRADE_MAXHP       = 1;
    private float UPGRADE_SPEED       = 0.5f;
    private float UPGRADE_LIGHTONTIME = 1f;

    private Rigidbody rigidbody;
    
    void Start()
    {
        hp = maxHP;
        last_max_position = transform.position.z;
        rigidbody = GetComponent<Rigidbody>();
        min_light = light.intensity;
    }

    void Update()
    {
        movement = Input.GetAxis("Vertical");
        if(transform.position.z > 0f || movement > 0)
        {
            rigidbody.MovePosition(transform.position + (transform.forward * movement * speed * Time.deltaTime));
        }
        if(Input.GetButtonDown("Fire1"))
            StartCoroutine("LightOn");
        if(Input.GetButtonUp("Fire1"))
        {    
            StopCoroutine("LightOn");
            StartCoroutine("LightOff");
        }
        if(hp <= 0)
            Death();
    }

    IEnumerator LightOn()
    {
        float light_strength = light.intensity;
        for(; light_strength < max_light; light_strength += 0.1f)
        {
            light.intensity = light_strength;
            yield return null;
        }
    }

    IEnumerator LightOff()
    {
        float light_strength = light.intensity;
        for(; light_strength > min_light; light_strength -= 0.1f)
        {
            light.intensity = light_strength;
            yield return null;
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
    }
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {  
            Debug.Log("HIT!");
            hp--;
        }
    }
}
