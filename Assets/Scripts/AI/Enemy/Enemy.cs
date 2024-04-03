using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Animator animator;

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private EnemyAwareness enemyAwareness;
    private FieldOfView fov;
    public NavMeshAgent Agent { get { return agent; } }
    public PatrolRoute patrolRoute;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAwareness = GetComponentInChildren<EnemyAwareness>();
        fov = GetComponentInChildren<FieldOfView>();

        PatrolState patrolState = new PatrolState(this, animator);
        ChaseState chaseState = new ChaseState(this, animator);
        AttackState attackState = new AttackState(this, animator);
        IdleState idleState = new IdleState(this, animator);

        At(patrolState, chaseState, new FuncPredicate(() => enemyAwareness.IsAlerted()));
        At(chaseState, patrolState, new FuncPredicate(() => !enemyAwareness.IsAlerted()));
        At(chaseState, attackState, new FuncPredicate(() => fov.CanSeePlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !fov.CanSeePlayer()));

        stateMachine.SetState(patrolState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);

    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
}
