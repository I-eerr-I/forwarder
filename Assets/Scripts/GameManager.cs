using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int day = 1;

    [Space]
    public GameObject player;
    public GameObject enemy;
    
    [Space]
    public UIManager uiManager;

    [Space]
    public GameObject hookWithLamp;

    [Space]
    public bool debug;

    [Space]
    public Transform levelEndObject;

    private bool noMonsterAnymore = false;
    private bool levelEnd = false;
    private bool upgraded = false;

    private PlayerController pc;

    private GameObject[] hooks;

    void Awake()
    {
        player = Instantiate(player);
        pc     = player.GetComponent<PlayerController>();
        SaveData data = SaveSystem.Load(0);
        if(data != null)
        {
            day           = data.day;
            pc.maxHP      = data.maxPlayerHP;
            pc.speed      = data.playerSpeed;
            pc.lightPower = data.lightPower;
        }
    }
    
    void Start()
    {
        uiManager.SetPlayerController(ref pc);
        uiManager.SetDay(day);
        hooks = GameObject.FindGameObjectsWithTag("Hook");
        GameObject randomHook = hooks[Random.Range(0, hooks.Length)];
        hookWithLamp = Instantiate(hookWithLamp);
        hookWithLamp.transform.position = randomHook.transform.position;
        hookWithLamp.transform.rotation = randomHook.transform.rotation;
        Destroy(randomHook);
    }

    void Update()
    {
            
            if(!noMonsterAnymore && !GameObject.FindWithTag("Enemy") && player.activeInHierarchy)
                InitEnemy();   
            if(levelEnd)
                EndLevel();
    }

    void InitEnemy()
    {
        EnemyParameters chars = enemy.GetComponent<EnemyParameters>();
        if(player.transform.position.z + chars.maxDistanceToPlayer < levelEndObject.position.z - 5f)
        {
            float distance = Random.Range(chars.minDistanceToPlayer, chars.maxDistanceToPlayer);
            enemy.transform.position = new Vector3(0, enemy.transform.position.y, player.transform.position.z + distance);
            GameObject game_enemy = Instantiate(enemy);
            EnemyParameters gameEnemyParameters = game_enemy.GetComponent<EnemyParameters>();
            gameEnemyParameters.SetPlayer(ref player);
            gameEnemyParameters.hp += day;
            gameEnemyParameters.speed += (((float)day)/10 * gameEnemyParameters.speed);
        }
        else
        {
            noMonsterAnymore = true;
        }
    }

    public void EndLevel()
    {
        levelEnd   = true;
        pc.enabled = false;
        uiManager.UpgradeMenu();
        if(upgraded)
        {
            day++;
            SaveSystem.Save(pc, this, 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpgradeHP()
    {
        pc.maxHP += pc.GetHPUpgrade();
        upgraded  = true;
    }

    public void UpgradeSpeed()
    {
        pc.speed += pc.GetSpeedUpgrade();
        upgraded  = true;
    }

    public void UpgradeLight()
    {
        pc.lightPower += pc.GetLightUpgrade();
        upgraded       = true;
    }

}
