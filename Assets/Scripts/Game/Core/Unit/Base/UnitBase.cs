using ProjectDawn.LocalAvoidance;
using System.Collections;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable, ICombatEntity
{
    [Header("Info")]
    public UnitStateEnum currentState;

    [Header("Component")]
    [SerializeField] private UnitVisual visual;
    [SerializeField] private UnitFeedback feedback;

    [Header("Agent")]
    [SerializeField] private Agent agent;
    [SerializeField] private FollowAgent followAgent;

    [Header("Value")]
    public float moveSpeed = 10f;
    public float stopDistance = 0.2f;

    [Header("Attack")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 0.2f;

    private bool isAttacking;
    private bool isDie;

    private ICombatEntity target;

    // Get set
    public bool IsDead => isDie;
    public Transform Transform => this.transform;

    public Agent Agent => agent;

    //======================== Setup ======================
    public virtual void Init()
    {
        visual.Set();

        // Agent
        agent.Speed = moveSpeed;
        agent.StopDistance = stopDistance;
    }

    #region Behavior
    public void StartAction()
    {
        isAttacking = false;
    }

    public void StopAction()
    {
        StopAllCoroutines();
        StopMoving();
        visual.ChangeState(UnitStateEnum.Idle);
    }

    //======================== Idle ======================
    public void Idle()
    {
        visual.ChangeState(currentState);
    }

    //======================== Move ======================
    private void Moving()
    {
        if(target == null) return;

        visual.ChangeState(UnitStateEnum.Move);
        visual.StartRotateMove();
        visual.Facing(target.Transform);
    }

    private void StopMoving()
    {
        visual.StopRotateMove();
        visual.ChangeState(UnitStateEnum.Idle);
    }

    //======================== Attack ======================
    public void Attack()
    {
        if (isAttacking) return;

        StartCoroutine(AttackIEnum());
    }

    private IEnumerator AttackIEnum()
    {
        isAttacking = true;
        visual.ChangeState(UnitStateEnum.Attack);
        DealDamage();
        feedback.AttackForce(target.Transform);

        yield return new WaitForSeconds(attackCooldown);
        visual.ChangeState(UnitStateEnum.Idle);

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            targetLayer
        );

        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }

    //======================== Die ======================
    public void Die()
    {
        isDie = true;

        target = null;
        gameObject.SetActive(false);

        StopAllCoroutines();
        visual.ChangeState(UnitStateEnum.Die);
    }

    //======================== Take Damage ======================
    public virtual void TakeDamage(int damage)
    {
        //Debug.Log("AAA");
    }

    #endregion

    #region Update State

    //======================== Update ======================
    public void SetTarget(UnitBase target)
    {
        this.target = target;
        followAgent.TargetAgent = target.GetComponent<Agent>();

        if (target == null)
        {
            StopAllCoroutines();
            return;
        }

        UpdateState();
    }

    public void ChangeState(UnitStateEnum newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    public void UpdateState()
    {
        if (isDie) return;

        switch (currentState)
        {
            case UnitStateEnum.Idle:
                UpdateIdle();
                break;

            case UnitStateEnum.Move:
                UpdateMove();
                break;

            case UnitStateEnum.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        if (target == null) return;

        float dist = Vector3.Distance(transform.position, target.Transform.position);

        if (dist > stopDistance)
            ChangeState(UnitStateEnum.Move);
        else
            ChangeState(UnitStateEnum.Attack);

        Idle();
    }

    void UpdateMove()
    {
        if (target == null)
        {
            StopMoving();
            ChangeState(UnitStateEnum.Idle);
            return;
        }

        if (agent.IsStopped)
        {
            StopMoving();
            ChangeState(UnitStateEnum.Attack);
            return;
        }

        Moving();
    }

    void UpdateAttack()
    {
        if (target == null)
        {
            ChangeState(UnitStateEnum.Idle);
            return;
        }

        if (!agent.IsStopped)
        {
            ChangeState(UnitStateEnum.Move);
            return;
        }

        // Logic attack
        Attack();
    }

    #endregion

    #region Combat 

    public void SetTarget(ICombatEntity target)
    {
        this.target = target;
        followAgent.TargetAgent = target.Agent;

        if (target == null)
        {
            StopAllCoroutines();
            return;
        }
    }

    public ICombatEntity GetTarget()
    {
        return target;
    }

    #endregion

    #region Editor

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    #endregion
}
