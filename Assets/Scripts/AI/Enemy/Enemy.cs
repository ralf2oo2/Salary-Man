using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource audioSource;

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private EnemyAwareness enemyAwareness;
    private FieldOfView fov;
    private Health health;
    private RigBuilder rigBuilder;

    private Gun gun;

    private bool isAlive = true;
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
        health = GetComponentInChildren<Health>();
        rigBuilder = GetComponentInChildren<RigBuilder>();
        gun = GetComponentInChildren<Gun>();

        rigBuilder.enabled = false;

        health.OnDeath += () =>
        {
            isAlive = false;
        };

        PatrolState patrolState = new PatrolState(this, animator, audioSource);
        ChaseState chaseState = new ChaseState(this, animator, audioSource);
        AttackState attackState = new AttackState(this, animator, audioSource, rigBuilder, gun);
        IdleState idleState = new IdleState(this, animator, audioSource);
        DeadState deadState = new DeadState(this, animator, audioSource);

        At(patrolState, chaseState, new FuncPredicate(() => enemyAwareness.IsAlerted()));
        At(chaseState, patrolState, new FuncPredicate(() => !enemyAwareness.IsAlerted()));
        At(chaseState, attackState, new FuncPredicate(() => fov.CanSeePlayer()));
        At(attackState, chaseState, new FuncPredicate(() => !fov.CanSeePlayer()));
        Any(deadState, new FuncPredicate(() => !isAlive));

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
