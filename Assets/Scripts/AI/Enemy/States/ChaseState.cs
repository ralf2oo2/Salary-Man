using UnityEngine;

public class ChaseState : EnemyBaseState
{
    GameObject player;
    public ChaseState(Enemy enemy, Animator animator, AudioSource audioSource) : base(enemy, animator , audioSource)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void OnEnter()
    {
        animator.CrossFade(runFwdHash, crossFadeDuration);
        enemy.Agent.speed = 3;
    }

    public override void FixedUpdate()
    {

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        Debug.Log("teest");
        enemy.Agent.SetDestination(player.transform.position);
    }
}