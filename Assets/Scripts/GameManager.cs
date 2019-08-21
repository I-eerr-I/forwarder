using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;
    public float minTunnelLength;
    public float maxTunnelLength;
    public GameObject wall;

    [Space]
    public UIManager uiManager;

    [Space]
    public bool debug;

    private GameObject wallHolder;
    private PlayerController pc;

    void Awake()
    {
        wallHolder = new GameObject("Wall Holder");
        player = Instantiate(player);
        pc = player.GetComponent<PlayerController>();
        uiManager.SetPlayer(ref player, ref pc);
        InitWalls();
    }

    void Start()
    {
        
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

    void InitWalls()
    {
        float tunnelLength   = Random.Range(minTunnelLength, maxTunnelLength);

        GameObject startWall = Instantiate(wall, wall.transform.position, Quaternion.identity);
        startWall.name = "StartWall";
        SpriteRenderer startSprite = startWall.GetComponent<SpriteRenderer>();
        float startX = startWall.transform.position.x;
        float startY = startWall.transform.position.y;
        float startZ = startWall.transform.position.z;
        startWall.transform.Rotate(0, 180f, 0);
        AttachToWallHolder(startWall);

        GameObject endWall   = Instantiate(wall);
        endWall.name = "EndWall";
        endWall.transform.position = new Vector3(startX, startY, startZ + tunnelLength);
        endWall.transform.Rotate(0, 180f, 0);
        AttachToWallHolder(endWall);

        GameObject leftWall = Instantiate(wall);
        leftWall.name = "LeftWall";
        SpriteRenderer leftSprite = leftWall.GetComponent<SpriteRenderer>();
        leftWall.transform.Rotate(0,-90f,0);
        leftSprite.size = new Vector2(tunnelLength, leftSprite.size.y);
        leftWall.transform.position = new Vector3(startSprite.size.x/2, startY, leftSprite.size.x/2);
        AttachToWallHolder(leftWall);

        GameObject rightWall = Instantiate(leftWall);
        rightWall.name = "RightWall";
        rightWall.transform.Rotate(0,180f,0);
        rightWall.transform.position = new Vector3(-leftWall.transform.position.x, startY, leftWall.transform.position.z);
        AttachToWallHolder(rightWall);
    }

    void AttachToWallHolder(GameObject wall)
    {
        wall.transform.SetParent(wallHolder.transform, true);
    }

}
