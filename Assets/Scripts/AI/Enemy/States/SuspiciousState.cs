using UnityEngine;
using UnityEngine.AI;

public class SuspiciousState : EnemyBaseState
{
    private bool movingToSuspiciousPoint = false;
    private int walkRadius = 15;

    public SuspiciousState(Enemy enemy, Animator animator, AudioSource audioSource) : base(enemy, animator, audioSource)
    {

    }

    public override void OnEnter()
    {
        animator.CrossFade(walkFwdSusHash, crossFadeDuration);
        enemy.Agent.speed = 2f;
        if(enemy.HasSuspiciousPoint())
        {
            movingToSuspiciousPoint = true;
            enemy.Agent.SetDestination(enemy.GetSuspiciousPoint());
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
        if(movingToSuspiciousPoint && enemy.Agent.remainingDistance < 0.2f)
        {
            movingToSuspiciousPoint = false;
            enemy.Agent.SetDestination(RandomNavmeshLocation());
            Debug.Log("Reached point");
        }
        if(!movingToSuspiciousPoint)
        {
            if(enemy.Agent.remainingDistance < 0.2f)
            {
                enemy.Agent.SetDestination(RandomNavmeshLocation());
                Debug.Log("Reached roam point");
            }
        }
    }

    public Vector3 RandomNavmeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitCircle * walkRadius;
        randomPosition += enemy.transform.position;

        if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
