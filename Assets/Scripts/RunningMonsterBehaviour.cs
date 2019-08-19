using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningMonsterBehaviour : EnemyBehaviour
{

    protected override void Action()
    {
        rb.MovePosition(transform.position - (transform.forward * speed * Time.deltaTime));
    }
}
