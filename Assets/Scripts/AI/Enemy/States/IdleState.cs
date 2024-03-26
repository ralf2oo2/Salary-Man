using UnityEngine;

public class IdleState : EnemyBaseState
{
    public IdleState(Enemy enemy, Animator animator) : base(enemy, animator)
    {
        
    }

    public override void OnEnter()
    {
        animator.CrossFade(idleHash, crossFadeDuration);
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
