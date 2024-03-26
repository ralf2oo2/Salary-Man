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
    public NavMeshAgent Agent { get { return agent; } }
    public PatrolRoute patrolRoute;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        PatrolState patrolState = new PatrolState(this, animator);
        Any(patrolState, new FuncPredicate(() => true));
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
