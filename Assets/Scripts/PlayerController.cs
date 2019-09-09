using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Light light;
    public SphereCollider lightPowerField;

    public int   maxHP         = 3;
    public float speed         = 0.5f;
    public float lightPower    = 1f;

    [Space]
    public int hp;
    
    private float minLight;
    private float maxLight          = 3f;
    private float maxLightCountdown = 5f;
    private float lightCountdown    = 0f;
    private float maxLightTime      = 5f;
    private bool  isLightOn;
    
    private bool  decreaseHP;

    private float movement;

    private Rigidbody   rigidbody;
    private BoxCollider boxCollider;
    
    void Start()
    {
        hp          = maxHP;
        rigidbody   = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        minLight    = light.intensity;
        lightPowerField.enabled = false;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !isLightOn && lightCountdown <= 0f)
        {
            isLightOn      = true;
            StartCoroutine("LightOn");
        }
        if(Input.GetButtonUp("Fire1") || !isLightOn)
        {    
            isLightOn      = false;
            StartCoroutine("LightOff");
        }
        if(isLightOn)
        {
            lightCountdown += 1f * Time.deltaTime;
            if(lightCountdown > maxLightCountdown)
            {
                lightCountdown = maxLightCountdown;
                isLightOn      = false;
            }
        }
        if(lightCountdown > 0f && !isLightOn)
            lightCountdown -= 1f * Time.deltaTime;
        else if(lightCountdown < 0f)
            lightCountdown = 0f;
        lightPowerField.enabled = isLightOn;
        
        movement = Input.GetAxis("Vertical");
        if(transform.position.z > 0f || movement > 0)
        {
            rigidbody.MovePosition(transform.position + (transform.forward * movement * speed * Time.deltaTime));
        }
        
        if(decreaseHP)
        {
            hp--;
            decreaseHP = false;   
        }
        if(hp <= 0)
            Death();
    }

    public float GetLightCountdown()
    {
        return lightCountdown;
    }

    public bool GetDecreaseHP()
    {
        return decreaseHP;
    }

    public float GetMaxLightCountdown()
    {
        return maxLightCountdown;
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
    
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Lamp"))
        {
            if(Input.GetButtonDown("Jump") && hp < maxHP)
            {
                hp++;
                col.enabled = false;
                col.gameObject.GetComponent<Light>().enabled = false;
                SpriteRenderer turnedOffLamp = GameObject.FindGameObjectWithTag("Turned Off Lamp").GetComponent<SpriteRenderer>();
                col.gameObject.GetComponent<SpriteRenderer>().sprite = turnedOffLamp.sprite;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            decreaseHP = true;
        }
    }

    

}
