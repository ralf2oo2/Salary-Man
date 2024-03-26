using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public int waypointIndex;

    public PatrolState(Enemy enemy, Animator animator) : base(enemy, animator)
    {

    }

    public override void OnEnter()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void Update()
    {
        PatrolCycle();
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            if (waypointIndex < enemy.patrolRoute.waypoints.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            enemy.Agent.SetDestination(enemy.patrolRoute.waypoints[waypointIndex].position);
        }
    }
}
