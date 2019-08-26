using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private Transform player;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = player.position;
    }
}
