using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public int waypointIndex = 0;

    public PatrolState(Enemy enemy, Animator animator, AudioSource audioSource) : base(enemy, animator, audioSource)
    {

    }

    public override void OnEnter()
    {
        animator.CrossFade(walkFwdHash, crossFadeDuration);
        enemy.Agent.speed = 1;
        if(enemy.patrolRoute != null)
        {
            enemy.Agent.SetDestination(enemy.patrolRoute.waypoints[waypointIndex].position);
        }
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
        if (enemy.patrolRoute == null || enemy.patrolRoute.waypoints == null) return;
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            if (waypointIndex < enemy.patrolRoute.waypoints.Count - 1 && enemy.patrolRoute.waypoints.Count > 0)
            {
                waypointIndex++;
            }
            else
            {
                waypointIndex = 0;
            }
            if (enemy.patrolRoute == null || enemy.patrolRoute.waypoints == null || waypointIndex > enemy.patrolRoute.waypoints.Count - 1) return;
            enemy.Agent.SetDestination(enemy.patrolRoute.waypoints[waypointIndex].position);
        }
    }
}
