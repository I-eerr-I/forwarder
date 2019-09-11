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

    private float lightOnTime         = 5f;
    private float lightCountdownTime  = 5f;
    private float lightCurrentTime    = 0f;
    private bool  powerLightOn        = false;
    private bool  lightCountdown      = false;

    private bool decreaseHP;
    private bool lampHelp;

    private float movement;

    private Rigidbody   rigidbody;
    private BoxCollider boxCollider;
    
    void Start()
    {
        hp          = maxHP;
        rigidbody   = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        lightPowerField.enabled = false;
    }

    void Update()
    {
        PowerLightAction();        
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

    void PowerLightAction()
    {
        if(Input.GetButtonDown("Fire1") && !lightCountdown)
        {
            powerLightOn = true;
        }
        if(powerLightOn)
        {
            lightCurrentTime += Time.deltaTime;
        }
        if(lightCurrentTime >= lightOnTime)
        {
            powerLightOn   = false;
            lightCountdown = true; 
        }
        if(Input.GetButtonUp("Fire1") && powerLightOn)
        {
            powerLightOn = false;
            lightCurrentTime = 0f;
        }
        if(lightCountdown)
        {
            lightCurrentTime -= Time.deltaTime;
        }
        if(lightCountdown && lightCurrentTime <= lightOnTime - lightCountdownTime)
        {
            lightCountdown   = false;
            lightCurrentTime = 0f;
        }
        lightPowerField.enabled = powerLightOn;
    }

    public bool GetDecreaseHP()
    {
        return decreaseHP;
    }

    public float GetCurrentHP()
    {
        return hp;
    }

    public bool GetLampHelp()
    {
        return lampHelp;
    }

    public bool GetPowerLightOn()
    {
        return powerLightOn;
    }

    public bool GetPowerLightCountdown()
    {
        return lightCountdown;
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
                lampHelp    = true;
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
