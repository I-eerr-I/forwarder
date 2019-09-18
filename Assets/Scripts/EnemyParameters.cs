using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameters : MonoBehaviour
{

    public float speed;
    public float power;
    public float deadRunDistance;
    public float hp = 5f;
    public float sightDistance;
    public AudioClip idlingSound;
    public AudioClip actionSound;

    public float minDistanceToPlayer;
    public float maxDistanceToPlayer;

    private GameObject player;

    public void SetPlayer(ref GameObject player)
    {
        this.player = player;
    }

    public ref GameObject GetPlayer()
    {
        return ref player;
    }

}
