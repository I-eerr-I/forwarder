using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    [Space]
    public GameObject hookWithLamp;

    [Space]
    public bool debug;

    private PlayerController pc;

    private GameObject[] hooks;

    void Awake()
    {
        player = Instantiate(player);
        pc     = player.GetComponent<PlayerController>();
    }
    
    void Start()
    {
        hooks = GameObject.FindGameObjectsWithTag("Hook");
        GameObject randomHook = hooks[Random.Range(0, hooks.Length)];
        hookWithLamp = Instantiate(hookWithLamp);
        hookWithLamp.transform.position = randomHook.transform.position;
        hookWithLamp.transform.rotation = randomHook.transform.rotation;
        Destroy(randomHook);
        Debug.Log("Scene:" + SceneManager.GetActiveScene().buildIndex.ToString());
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
