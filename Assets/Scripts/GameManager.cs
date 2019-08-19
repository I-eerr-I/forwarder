using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;
    public GameObject startWall;
    public GameObject endWall;
    public GameObject[] walls;
    public GameObject cornerWall;
    public float minTunnelLength;
    public float maxTunnelLength;

    private GameObject wallHolder;

    private bool enemyActive;

    void Awake()
    {
        wallHolder = new GameObject("Wall Holder");
        player = Instantiate(player);
        InitWalls();
    }

    void Update()
    {
        if(!GameObject.FindWithTag("Enemy") && player.activeInHierarchy)
            InitRandomEnemy();
    }

    void InitRandomEnemy()
    {
        foreach(GameObject enemy in enemies)
        {
            AppearanceChars chars = enemy.GetComponent<AppearanceChars>();
            float probability = Random.Range(0f,1f);
            PlayerController pc = player.GetComponent<PlayerController>();
            Debug.Log("Analising enemy:" + enemy.name);
            Debug.Log("Probability: "+ probability.ToString());
            Debug.Log("Real Prob: " + (chars.notActivityProbability + pc.luck).ToString());
            Debug.Log("Result: " + (probability > chars.notActivityProbability + pc.luck && pc.level > chars.playerLevelToStart).ToString());
            if(probability > chars.notActivityProbability + pc.luck && pc.level >= chars.playerLevelToStart)
            {
                float distance = Random.Range(chars.minDistanceToPlayer, chars.maxDistanceToPlayer);
                enemy.transform.position = new Vector3(0, enemy.transform.position.y, player.transform.position.z + distance);
                GameObject game_enemy = Instantiate(enemy);
                Debug.Log("Enemy was born at " + game_enemy.transform.position.z.ToString());
                Debug.Log("Distance to player: " + distance.ToString());
                break;
            }
        }
    }

    void InitWalls()
    {
        float tunnelLength = Random.Range(minTunnelLength, maxTunnelLength);
        
        for(int i = 1; i < tunnelLength/cornerWall.transform.localScale.z; i++)
        {
            int sign = (Random.Range(0f,1f) > 0.5)? 1 : -1;
            GameObject game_cornerWall = Instantiate(cornerWall);
            game_cornerWall.transform.position = new Vector3(sign * game_cornerWall.transform.position.x,
                                                             game_cornerWall.transform.position.y,
                                                             game_cornerWall.transform.position.z * i);
            AttachToWallHolder(game_cornerWall);
        }

        GameObject game_startWall = Instantiate(startWall);
        GameObject game_endWall   = Instantiate(endWall);
        game_endWall.transform.position += new Vector3(0, 0, tunnelLength);
        AttachToWallHolder(game_endWall);
        AttachToWallHolder(game_startWall);

        foreach(GameObject wall in walls)
        {
            GameObject game_wall = Instantiate(wall);
            game_wall.transform.position   += new Vector3(0, 0, tunnelLength);
            game_wall.transform.localScale += new Vector3(0, 0, 2*tunnelLength);
            AttachToWallHolder(game_wall);
        }
    }

    void AttachToWallHolder(GameObject wall)
    {
        wall.transform.SetParent(wallHolder.transform, true);
    }
}
