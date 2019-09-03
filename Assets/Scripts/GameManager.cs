using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;

    [Space]
    public UIManager uiManager;

    [Space]
    public bool debug;

    private PlayerController pc;

    void Awake()
    {
        player = Instantiate(player);
        pc = player.GetComponent<PlayerController>();
        uiManager.SetPlayer(ref player, ref pc);
    }

    void Update()
    {
        if(!GameObject.FindWithTag("Enemy") && player.activeInHierarchy)
            InitRandomEnemy();
    }

    void InitRandomEnemy()
    {
        if(enemies.Length == 1)
        {
            GameObject enemy = enemies[0];
            AppearanceChars chars = enemy.GetComponent<AppearanceChars>();
            if(pc.level >= chars.playerLevelToStart)
                InitEnemy(enemy, chars);
        }
        else
        {
            foreach(GameObject enemy in enemies)
            {
                AppearanceChars chars = enemy.GetComponent<AppearanceChars>();
                float probability = Random.Range(0f,1f);
                if(debug)
                {
                    Debug.Log("Analising enemy:" + enemy.name);
                    Debug.Log("Probability: "+ probability.ToString());
                    Debug.Log("Real Prob: " + (chars.notActivityProbability + pc.luck).ToString());
                    Debug.Log("Result: " + (probability > chars.notActivityProbability + pc.luck && pc.level > chars.playerLevelToStart).ToString());
                }
                if(probability > chars.notActivityProbability + pc.luck && pc.level >= chars.playerLevelToStart)
                {
                    InitEnemy(enemy, chars);
                    break;
                }
            }
        }
    }

    void InitEnemy(GameObject enemy, AppearanceChars chars)
    {
        float distance = Random.Range(chars.minDistanceToPlayer, chars.maxDistanceToPlayer);
        enemy.transform.position = new Vector3(0, enemy.transform.position.y, player.transform.position.z + distance);
        GameObject game_enemy = Instantiate(enemy);
        Debug.Log("Enemy was born at " + game_enemy.transform.position.z.ToString());
        Debug.Log("Distance to player: " + distance.ToString());
        game_enemy.GetComponent<AppearanceChars>().SetPlayer(ref player);
    }

}
