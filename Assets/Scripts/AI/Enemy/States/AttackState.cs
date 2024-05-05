using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AttackState : EnemyBaseState
{
    private RigBuilder rigBuilder;
    private NpcGun gun;
    public AttackState(Enemy enemy, Animator animator, AudioSource audioSource, RigBuilder rigBuilder, NpcGun gun) : base(enemy, animator, audioSource)
    {
        this.rigBuilder = rigBuilder;
        this.gun = gun;
    }

    public override void OnEnter()
    {
        animator.CrossFade(rifleAimHash, crossFadeDuration);
        enemy.Agent.SetDestination(enemy.transform.position);
        rigBuilder.enabled = true;
    }

    public override void FixedUpdate()
    {

    }

    public override void OnExit()
    {
        rigBuilder.enabled = false;
    }

    public override void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        enemy.transform.LookAt(new Vector3(playerPos.x, enemy.transform.position.y, playerPos.z));
        if(gun.currentAmmo > 0)
        {
            gun.Shoot();
        }
        else
        {
            gun.StartReload();
        }
    }
}
