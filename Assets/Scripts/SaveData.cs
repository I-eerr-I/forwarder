using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int   day;
    public int   maxPlayerHP;
    public float playerSpeed;
    public float lightPower;

    public SaveData(PlayerController playerController, GameManager gameManager)
    {
        day         = gameManager.day;
        maxPlayerHP = playerController.maxHP;
        playerSpeed = playerController.speed;
        lightPower  = playerController.lightPower; 
    }
}
