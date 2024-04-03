using UnityEngine;

public class DeadState : EnemyBaseState
{
    public DeadState(Enemy enemy, Animator animator) : base(enemy, animator)
    {

    }

    public override void OnEnter()
    {
        animator.CrossFade(deadHash, crossFadeDuration);
        enemy.Agent.SetDestination(enemy.transform.position);
    }

    public override void FixedUpdate()
    {

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {

    }
}
