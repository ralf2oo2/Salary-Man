using UnityEngine;

public class DeadState : EnemyBaseState
{
    private AudioClip deathClip;
    public DeadState(Enemy enemy, Animator animator, AudioSource audioSource) : base(enemy, animator, audioSource)
    {
        deathClip = Resources.Load<AudioClip>("Sound/GuardDeath");
    }

    public override void OnEnter()
    {
        animator.CrossFade(deadHash, crossFadeDuration);
        enemy.Agent.SetDestination(enemy.transform.position);
        audioSource.clip = deathClip;
        audioSource.Play();
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
