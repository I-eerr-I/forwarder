using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    [Space]
    public bool debug;

    private PlayerController pc;

    void Awake()
    {
        player = Instantiate(player);
        pc = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(!GameObject.FindWithTag("Enemy") && player.activeInHierarchy)
            InitEnemy();
    }

    void InitEnemy()
    {
        AppearanceChars chars = enemy.GetComponent<AppearanceChars>();
        float distance = Random.Range(chars.minDistanceToPlayer, chars.maxDistanceToPlayer);
        enemy.transform.position = new Vector3(0, enemy.transform.position.y, player.transform.position.z + distance);
        GameObject game_enemy = Instantiate(enemy);
        game_enemy.GetComponent<AppearanceChars>().SetPlayer(ref player);
    }

}
