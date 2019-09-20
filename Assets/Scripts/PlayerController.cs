using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Light")]
    public Light lampLight;
    public SphereCollider lightPowerField;

    [Header("Characteristics")]
    public int   maxHP         = 3;
    public float speed         = 0.5f;
    public float lightPower    = 1f;

    [Header("Debug Info")]
    public int hp;

    int   hpUpgrade    = 1;
    float speedUpgrade = 0.1f;
    float lightUpgrade = 0.5f; 

    float lightOnTime         = 5f;
    float lightCountdownTime  = 5f;
    float lightCurrentTime    = 0f;
    bool  powerLightOn        = false;
    bool  lightCountdown      = false;

    bool decreaseHP;
    bool lampHelp;

    float movement;

    Rigidbody   playerRigidbody;
    BoxCollider boxCollider;
    
    void Start()
    {
        hp                = maxHP;
        playerRigidbody   = GetComponent<Rigidbody>();
        boxCollider       = GetComponent<BoxCollider>();
        lightPowerField.enabled = false;
    }

    void Update()
    {
        PowerLightAction();        
        movement = Input.GetAxis("Vertical");
        if(transform.position.z > 0f || movement > 0)
        {
            playerRigidbody.MovePosition(transform.position + (transform.forward * movement * speed * Time.deltaTime));
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

    public int GetHPUpgrade()
    {
        return hpUpgrade;
    }

    public float GetSpeedUpgrade()
    {
        return speedUpgrade;
    }

    public float GetLightUpgrade()
    {
        return lightUpgrade;
    }

    public Light GetLightComponent()
    {
        return lampLight;
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
