using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Light light;

    public int   maxHP         = 3;
    public float speed         = 0.5f;
    public float lightOnTime   = 5f;
    
    private int hp;
    private float maxLight = 3f;
    private float minLight;
    private float startLightTime;
    private bool  isLightOn;

    private float movement;

    private Rigidbody rigidbody;
    
    void Start()
    {
        hp        = maxHP;
        rigidbody = GetComponent<Rigidbody>();
        minLight  = light.intensity;
    }

    void Update()
    {
        movement = Input.GetAxis("Vertical");
        if(transform.position.z > 0f || movement > 0)
        {
            rigidbody.MovePosition(transform.position + (transform.forward * movement * speed * Time.deltaTime));
        }
        if(Input.GetButtonDown("Fire1"))
        {
            startLightTime = Time.time;
            isLightOn = true;
            StartCoroutine("LightOn");
        }
        if(Input.GetButtonUp("Fire1") || (isLightOn && Time.time - startLightTime > lightOnTime))
        {    
            isLightOn = false;
            Debug.Log("Time:" + (Time.time - startLightTime).ToString());
            StartCoroutine("LightOff");
        }
        if(hp <= 0)
            Death();
    }

    IEnumerator LightOn()
    {
        float lightStrength = light.intensity;
        for(; lightStrength < maxLight; lightStrength += 0.1f)
        {
            light.intensity = lightStrength;
            yield return null;
        }
    }

    IEnumerator LightOff()
    {
        StopCoroutine("LightOn");
        float lightStrength = light.intensity;
        for(; lightStrength > minLight; lightStrength -= 0.1f)
        {
            light.intensity = lightStrength;
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
