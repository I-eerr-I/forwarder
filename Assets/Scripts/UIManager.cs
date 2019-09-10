using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float cameraAnimationAmplitude         = 0.02f;
    public float cameraAnimationSpeed             = 20f;
    public float cameraRotationAnimationAmplitude = 0.01f;
    public float cameraRotationAnimationSpeed     = 10f;
    public float cameraHPAnimationAmplitude       = 0.02f;
    public float cameraHPAnimationSpeed           = 100f;

    private PlayerController playerController;
    private Transform player;
    private Transform camera;
    private Animator playerLight;

    public void SetPlayerController(ref PlayerController playerController)
    {
        this.playerController = playerController; 
        player                = playerController.gameObject.transform;
        playerLight           = playerController.light.GetComponent<Animator>(); 
    }

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        playerLight.SetBool("Decrease HP", playerController.GetDecreaseHP());
        playerLight.SetBool("Power Light On", Input.GetButtonDown("Fire1"));
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
