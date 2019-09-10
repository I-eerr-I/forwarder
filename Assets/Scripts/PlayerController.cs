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
    
    private bool decreaseHP;
    private bool lampHelp;
    private bool powerLightOn;

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
        if(Input.GetButtonDown("Fire1"))
        {
            powerLightOn = true;
        }
        if(Input.GetButtonUp("Fire1"))
        {
            powerLightOn = false;
        }

        lightPowerField.enabled = powerLightOn;
        
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
