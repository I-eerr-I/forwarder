using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public int   maxHP         = 1;
    public float speed         = 1f;
    public float learnRate     = 0.5f;
    public float lightStrength = 3f;
    public float luck          = 0.1f;

    public float xp            = 0f;
    public int   level         = 1;
    public int   xpPoints      = 0;
    
    private int hp;

    private float last_max_position;

    private ArrayList upgradable = new ArrayList();

    private int   UPGRADE_MAXHP     = 1;
    private float UPGRADE_SPEED     = 0.5f;
    private float UPGRADE_LEARNRATE = 0.2f;
    private float UPGRADE_LIGHTSTRENGTH = 1;
    private float UPGRADE_LUCK      = 0.01f;

    private Rigidbody rigidbody;
    private Light light;

    void Start()
    {
        InitUpgradable();
        xp = 0f;
        hp = maxHP;
        last_max_position = transform.position.z;
        rigidbody = GetComponent<Rigidbody>();
        light     = GetComponent<Light>();
    }

    void Update()
    {
        light.range     = lightStrength;
        light.intensity = lightStrength;
        rigidbody.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime));
        UpdateXP();
        LevelUp();
        RandomUpgrade();
        if(hp <= 0)
            Death();
    }

    void InitUpgradable()
    {
        upgradable.Add(maxHP);
        upgradable.Add(speed);
        upgradable.Add(learnRate);
        upgradable.Add(lightStrength);
        upgradable.Add(luck);
    }

    void RandomUpgrade()
    {
        if(xpPoints > 0)
        {
            int index = Random.Range(0, upgradable.Count);
            switch(index)
            {
                case 0:
                    maxHP += UPGRADE_MAXHP;
                    Debug.Log("Upgraded Max HP");
                    break;
                case 1:
                    speed += UPGRADE_SPEED;
                    Debug.Log("Upgraded Speed");
                    break;
                case 2:
                    learnRate += UPGRADE_LEARNRATE;
                    Debug.Log("Upgraded learn rate");
                    break;
                case 3:
                    lightStrength += UPGRADE_LIGHTSTRENGTH;
                    Debug.Log("Upgraded light strength");
                    break;
                case 4:
                    luck += UPGRADE_LUCK;
                    Debug.Log("Upgraded luck");
                    break;
            }
            xpPoints--;
        }
    }

    void UpdateXP()
    {
        if(transform.position.z > last_max_position)
        {
            xp += learnRate*(transform.position.z - last_max_position);
            last_max_position = transform.position.z;
        }
    }

    void LevelUp()
    {
        if(xp >= level / learnRate)
        {
            Debug.Log("XP: " + xp.ToString());
            xp = 0f;
            level++;
            xpPoints++;
            Debug.Log("LEVEL: " + level.ToString());
            Debug.Log("XPPOINTS: " + xpPoints.ToString());
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
