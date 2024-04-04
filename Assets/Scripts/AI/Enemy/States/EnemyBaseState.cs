using Platformer;
using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected readonly Enemy enemy;
    protected readonly Animator animator;
    protected readonly AudioSource audioSource;

    protected static readonly int walkFwdHash = Animator.StringToHash("WalkFWD");
    protected static readonly int runFwdHash = Animator.StringToHash("RunFWD");
    protected static readonly int idleHash = Animator.StringToHash("Idle");
    protected static readonly int rifleAimHash = Animator.StringToHash("RifleAim");
    protected static readonly int deadHash = Animator.StringToHash("Dead");

    protected const float crossFadeDuration = 0.1f;

    public EnemyBaseState(Enemy enemy, Animator animator, AudioSource audioSource)
    {
        this.enemy = enemy;
        this.animator = animator;
        this.audioSource = audioSource;
    }

    public virtual void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
