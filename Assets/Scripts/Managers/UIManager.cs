using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public Text dayText;
    public GameObject dayEnd;

    // [Header("Player Health Parameters")]
    // public float middleHealthSoundPitch = 1f;
    // public float lowHealthSoundPitch    = 1.2f;

    [Header("Camera Animation")]
    public float cameraAnimationAmplitude            = 0.02f;
    public float cameraAnimationSpeed                = 20f;
    public float cameraMinRotationAnimationAmplitude = 0f;
    public float cameraRotationAnimationSpeed        = 10f;
    public float cameraRotationAdditionFromHP        = 0.05f;
    public float cameraMaxMonsterAnimationAmplitude  = 0.02f;
    public float cameraMonsterAnimationSpeed         = 100f;
    public float monsterScareLength = 1.5f;
    
    int  day;
    float currentCameraRotationAnimation;

    PlayerController playerController;
    Transform   player;
    Transform   playerCamera;
    Animator    playerLightAnimator;
    Light       playerLight;
    Light       playerHPLight;
    Animator    playerHPLightAnimator;
    AudioSource playerAudioSource;

    public void SetPlayerController(ref PlayerController playerController)
    {
        this.playerController = playerController; 
        player                = playerController.gameObject.transform;
        playerAudioSource     = player.GetComponent<AudioSource>();
        playerLight           = playerController.GetLightComponent();
        playerLightAnimator   = playerLight.GetComponent<Animator>();
        playerHPLight         = playerController.gameObject.GetComponent<Light>();
        playerHPLightAnimator = playerController.gameObject.GetComponent<Animator>();
        currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude;
    }

    public void SetDay(int day)
    {
        this.day = day;
        dayText.text = "Day " + day.ToString();
    }

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if(player.gameObject.activeSelf)
        {
            float hpPercents = 100f*playerController.GetCurrentHP()/playerController.maxHP;
            playerLightAnimator.SetBool("Power Light Turned On", Input.GetButtonDown("Fire1"));
            playerLightAnimator.SetBool("Power Light Still On", playerController.GetPowerLightOn());
            playerLightAnimator.SetBool("Power Light Countdown", playerController.GetPowerLightCountdown());
            playerHPLightAnimator.SetBool("Decrease HP", playerController.GetDecreaseHP());
            playerHPLightAnimator.SetFloat("HP Percents", hpPercents);
            
            if(hpPercents >= 75f)
            {
                if(playerAudioSource.isPlaying)
                    playerAudioSource.Stop();
                currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude;
            }
            if(50f <= hpPercents && hpPercents < 75f)
            {
                // playerAudioSource.pitch = middleHealthSoundPitch;
                if(!playerAudioSource.isPlaying)
                    playerAudioSource.Play();            
                currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude + cameraRotationAdditionFromHP;
            }
            else if(hpPercents < 50f)
            {
                // playerAudioSource.pitch = lowHealthSoundPitch;
                if(!playerAudioSource.isPlaying)
                    playerAudioSource.Play(); 
                currentCameraRotationAnimation = cameraMinRotationAnimationAmplitude + cameraRotationAdditionFromHP*2f;
            }
        }

        CameraAnimation();
    }

    void CameraAnimation()
    {
        float y = playerCamera.position.y;
        y = cameraAnimationAmplitude*Input.GetAxis("Vertical")*Mathf.Cos(player.position.z*cameraAnimationSpeed)+player.position.y;
        playerCamera.position = new Vector3(playerCamera.position.x, y, player.position.z);
        playerCamera.Rotate(0, 0, currentCameraRotationAnimation*Mathf.Cos(Time.time*cameraRotationAnimationSpeed));
    }

    public void UpgradeMenu()
    {
        if(!dayEnd.activeSelf)
            dayEnd.SetActive(true);
    }
}
