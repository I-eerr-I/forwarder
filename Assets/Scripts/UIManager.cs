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
    public float cameraAnimationAmplitude            = 0.02f;
    public float cameraAnimationSpeed                = 20f;
    public float cameraMinRotationAnimationAmplitude = 0f;
    public float cameraRotationAnimationSpeed        = 10f;
    public float cameraRotationAdditionFromHP        = 0.05f;
    public float cameraMaxMonsterAnimationAmplitude  = 0.02f;
    public float cameraMonsterAnimationSpeed         = 100f;
    public float monsterScareLength = 1.5f;
    
    private float currentCameraRotationAnimation;

    private PlayerController playerController;
    private Transform player;
    private Transform camera;
    private Animator  playerLightAnimator;
    private Light     playerLight;
    private Light     playerHPLight;
    private Animator  playerHPLightAnimator;

    public void SetPlayerController(ref PlayerController playerController)
    {
        this.playerController = playerController; 
        player                = playerController.gameObject.transform;
        playerLightAnimator   = playerController.light.GetComponent<Animator>();
        playerLight           = playerController.light;
        playerHPLight         = playerController.gameObject.GetComponent<Light>();
        playerHPLightAnimator = playerController.gameObject.GetComponent<Animator>();
        currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude;
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

            currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude;
        }
        if(50f <= hpPercents && hpPercents < 75f)
        {
            playerHPLight.color     = middleHealthColor;
            playerHPLight.intensity = middleHealthIntensity;
            currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude + cameraRotationAdditionFromHP;
        }
        else if(hpPercents < 50f)
        {
            playerHPLight.color     = lowHealthColor;
            playerHPLight.intensity = lowHealthIntensity; 
            currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude + cameraRotationAdditionFromHP*2f;
        }

        CameraAnimation();
    }

    void CameraAnimation()
    {
        float y = camera.position.y;
        y = cameraAnimationAmplitude*Input.GetAxis("Vertical")*Mathf.Cos(player.position.z*cameraAnimationSpeed)+player.position.y;
        camera.position = new Vector3(camera.position.x, y, player.position.z);
        camera.Rotate(0, 0, currentCameraRotationAnimation*Mathf.Cos(Time.time*cameraRotationAnimationSpeed));
    }

    

}
