using UnityEngine;

public class AttackState : EnemyBaseState
{
    public AttackState(Enemy enemy, Animator animator) : base(enemy, animator)
    {

    }

    public override void OnEnter()
    {
        animator.CrossFade(rifleAimHash, crossFadeDuration);
        enemy.Agent.SetDestination(enemy.transform.position);
        Debug.Log("easports");
    }

    public override void FixedUpdate()
    {

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        enemy.transform.LookAt(new Vector3(playerPos.x, enemy.transform.position.y, playerPos.z));
    }
}
