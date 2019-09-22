using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int day = 1;

    [Header("Entities")]
    public GameObject player;
    public GameObject enemy;
    
    [Header("UI")]
    public UIManager uiManager;

    [Header("Scene Objects")]
    public GameObject hookWithLamp;
    public Transform levelEndObject;

    bool noMonsterAnymore = false;
    bool levelEnd         = false;
    bool upgraded         = false;

    int enemyAppearanceCounter;

    PlayerController pc;

    GameObject[] hooks;

    void Awake()
    {
        enemyAppearanceCounter = 0;
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
        InitEnemy();
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
        if(chars.minAppearTimes + (day-1) < ++enemyAppearanceCounter)
        {
            noMonsterAnymore = true;
            return;
        }
        if(player.transform.position.z + chars.maxDistanceToPlayer < levelEndObject.position.z - 5f)
        {
            float minDistanceToPlayer = chars.minDistanceToPlayer - ((((float)day-1)/10f) * chars.minDistanceToPlayer);
            float maxDistanceToPlayer = chars.maxDistanceToPlayer - ((((float)day-1)/10f) * chars.maxDistanceToPlayer);
            float distance = Random.Range(minDistanceToPlayer, maxDistanceToPlayer);
            enemy.transform.position = new Vector3(0, enemy.transform.position.y, player.transform.position.z + distance);
            GameObject game_enemy = Instantiate(enemy);
            EnemyParameters gameEnemyParameters = game_enemy.GetComponent<EnemyParameters>();
            gameEnemyParameters.SetPlayer(ref player);
            gameEnemyParameters.hp += day-1;
            gameEnemyParameters.speed += ((((float)day-1)/10f) * gameEnemyParameters.speed);
            uiManager.SetEnemy(ref game_enemy);
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
