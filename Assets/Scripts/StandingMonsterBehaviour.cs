using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingMonsterBehaviour : EnemyBehaviour
{
    
    protected override void Action()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.forward, out hit, parameters.sightDistance))
        {
            isAction = true;
        }
        if(isAction)
            rb.MovePosition(transform.position - (transform.forward * parameters.speed * Time.deltaTime));
    }
}
