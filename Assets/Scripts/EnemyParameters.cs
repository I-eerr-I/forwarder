using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameters : MonoBehaviour
{

    [Header("Parameters")]
    public float speed;
    public float power;
    public float hp = 5f;
    public float sightDistance;

    [Header("Behaviour Parameters")]
    public float deadRunDistance;
    public float minDistanceToPlayer;
    public float maxDistanceToPlayer;
    public int   minAppearTimes;

    [Header("Sound")]
    public AudioClip idlingSound;
    public AudioClip actionSound;

    GameObject player;

    public void SetPlayer(ref GameObject player)
    {
        this.player = player;
    }

    public ref GameObject GetPlayer()
    {
        return ref player;
    }

}
