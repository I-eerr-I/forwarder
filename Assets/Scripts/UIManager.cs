using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Color middleHealthColor;
    public Color lowHealthColor;
    public float middleHealthIntensity = 1f;
    public float lowHealthIntensity    = 5f;

    [Space]
    public float cameraAnimationAmplitude         = 0.02f;
    public float cameraAnimationSpeed             = 20f;
    public float cameraRotationAnimationAmplitude = 0.01f;
    public float cameraRotationAnimationSpeed     = 10f;
    public float cameraHPAnimationAmplitude       = 0.02f;
    public float cameraHPAnimationSpeed           = 100f;

    private PlayerController playerController;
    private Transform player;
    private Transform camera;
    private Animator playerLightAnimator;
    private Light    playerLight;
    private Light    playerHPLight;
    private Animator playerHPLightAnimator;

    public void SetPlayerController(ref PlayerController playerController)
    {
        this.playerController = playerController; 
        player                = playerController.gameObject.transform;
        playerLightAnimator   = playerController.light.GetComponent<Animator>();
        playerLight           = playerController.light;
        playerHPLight         = playerController.gameObject.GetComponent<Light>();
        playerHPLightAnimator = playerController.gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        float hpPercents = 100f*playerController.GetCurrentHP()/playerController.maxHP;
        playerLightAnimator.SetBool("Decrease HP", playerController.GetDecreaseHP());
        playerLightAnimator.SetBool("Power Light Turned On", Input.GetButtonDown("Fire1"));
        playerLightAnimator.SetBool("Power Light Still On", playerController.GetPowerLightOn());
        playerLightAnimator.SetBool("Power Light Countdown", playerController.GetPowerLightCountdown());
        playerHPLightAnimator.SetFloat("HP Percents", hpPercents);
        
        if(hpPercents >= 75f)
        {
            Color playerHPLightColor = playerHPLight.color;
            playerHPLightColor.a     = 0f;
            playerHPLight.color      = playerHPLightColor;
        }
        if(50f <= hpPercents && hpPercents < 75f)
        {
            playerHPLight.color     = middleHealthColor;
            playerHPLight.intensity = middleHealthIntensity;
        }
        else if(hpPercents < 50f)
        {
            playerHPLight.color     = lowHealthColor;
            playerHPLight.intensity = lowHealthIntensity; 
        }

        CameraAnimation();
    }

    void CameraAnimation()
    {
        float x = camera.position.x;
        float y = camera.position.y;
        x       = (playerController.maxHP - playerController.GetCurrentHP())*cameraHPAnimationAmplitude*Mathf.Cos(Time.time*cameraHPAnimationSpeed);
        y       = cameraAnimationAmplitude*Input.GetAxis("Vertical")*Mathf.Cos(player.position.z*cameraAnimationSpeed)+player.position.y;
        camera.position = new Vector3(x, y, player.position.z);
        camera.Rotate(0, 0, Input.GetAxis("Vertical")*cameraRotationAnimationAmplitude*Mathf.Cos(player.position.z*cameraRotationAnimationSpeed));
    }

    

}
