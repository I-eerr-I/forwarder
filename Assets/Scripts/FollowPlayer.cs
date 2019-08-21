using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float animationSpeed = 3f;
    public float magnitude = 0.1f;

    private Transform player;
    private float start_y;
    private float movement;

    void Start()
    {
        start_y = transform.position.y;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = player.position;
    }
}
