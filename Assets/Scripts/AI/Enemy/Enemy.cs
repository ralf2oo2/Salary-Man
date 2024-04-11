using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private BoxCollider deadBodyCollider;

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private EnemyAwareness enemyAwareness;
    private AwarenessVisualizer awarenessVisualizer;
    private FieldOfView fov;
    private Health health;
    private RigBuilder rigBuilder;
    private CapsuleCollider capsuleCollider;

    private NpcGun gun;

    private Vector3 suspiciousPoint;

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
        awarenessVisualizer = GetComponentInChildren<AwarenessVisualizer>();
        fov = GetComponentInChildren<FieldOfView>();
        health = GetComponentInChildren<Health>();
        rigBuilder = GetComponentInChildren<RigBuilder>();
        gun = GetComponentInChildren<NpcGun>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        rigBuilder.enabled = false;

        PatrolState patrolState = new PatrolState(this, animator, audioSource);
        SuspiciousState suspiciousState = new SuspiciousState(this, animator, audioSource);
        ChaseState chaseState = new ChaseState(this, animator, audioSource);
        AttackState attackState = new AttackState(this, animator, audioSource, rigBuilder, gun);
        IdleState idleState = new IdleState(this, animator, audioSource);
        DeadState deadState = new DeadState(this, animator, audioSource);

        At(patrolState, chaseState, new FuncPredicate(() => enemyAwareness.IsAlerted()));
        At(suspiciousState, chaseState, new FuncPredicate(() => enemyAwareness.IsAlerted()));
        At(patrolState, suspiciousState, new FuncPredicate(() => enemyAwareness.IsSuspicious() && !enemyAwareness.IsAlerted()));
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
        if (health.health < 0 && isAlive)
        {
            isAlive = false;
            capsuleCollider.enabled = false;
            agent.height = 0;
            agent.updatePosition = false;
            agent.updateRotation = false;
            deadBodyCollider.enabled = true;
            awarenessVisualizer.enabled = false;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void setAsTarget()
    {
        gameObject.layer = LayerMask.NameToLayer("Target");
    }
    public void RemoveAwareness()
    {
        //Destroy(enemyAwareness);
    }
    public void SetSuspiciousPoint(Vector3 point)
    {
        suspiciousPoint = point;
    }
    public bool HasSuspiciousPoint()
    {
        return suspiciousPoint != null;
    }
    public Vector3 GetSuspiciousPoint()
    {
        return suspiciousPoint;
    }
}
