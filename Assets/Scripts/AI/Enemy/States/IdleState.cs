using UnityEngine;

public class IdleState : EnemyBaseState
{
    public IdleState(Enemy enemy, Animator animator, AudioSource audioSource) : base(enemy, animator, audioSource)
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
