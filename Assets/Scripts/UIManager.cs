using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator decreaseHP;
    public Image lightCountdown;
    

    private PlayerController playerController;

    public void SetPlayerController(ref PlayerController playerController)
    {

        this.playerController = playerController; 
    }

    void Start()
    {   
        decreaseHP.SetBool("Decrease HP", false);
        lightCountdown.enabled = false;
    }

    void Update()
    {
        decreaseHP.SetBool("Decrease HP", playerController.GetDecreaseHP());

        float playerLightCountdown = playerController.GetLightCountdown();
        if(playerLightCountdown > 0f)
        {
            lightCountdown.enabled    = true;
            Color lightCountdownColor = lightCountdown.color; 
            lightCountdownColor.a     = playerLightCountdown/playerController.GetMaxLightCountdown();
            lightCountdown.color      = lightCountdownColor;
        }
    }

    

}
