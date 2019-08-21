using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceChars : MonoBehaviour
{
    public float notActivityProbability;
    public int   playerLevelToStart;

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
