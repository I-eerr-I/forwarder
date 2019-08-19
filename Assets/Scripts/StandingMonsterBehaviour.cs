using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingMonsterBehaviour : EnemyBehaviour
{
    public float sightDistance;
    
    protected override void Action()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.forward, out hit, sightDistance))
        {
            rb.MovePosition(transform.position - (transform.forward * speed * Time.deltaTime));
        }
    }
}
