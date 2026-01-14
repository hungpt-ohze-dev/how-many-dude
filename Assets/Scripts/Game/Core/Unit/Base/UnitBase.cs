using ProjectDawn.LocalAvoidance;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable, ICombatEntity
{
    [Header("Info")]
    public UnitStateEnum currentState;

    [Header("Component")]
    [SerializeField] private UnitVisual visual;
    [SerializeField] private UnitFeedback feedback;
    [SerializeField] private BuffController buff;
    [SerializeField] private UnitAttack attack;

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
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float baseAttackCooldown = 0.5f;
    [SerializeField] private float attackSpeedMultiplier = 1f;

    private float attackTimer;
    private bool isAttacking;
    private bool isDie;

    [Header("Target")]
    [ShowInInspector]
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

        // Attack
        attackTimer = 0f;
    }

    #region Behavior

    //======================== Idle ======================
    public void Idle()
    {
        visual.ChangeState(currentState);
    }

    //======================== Move ======================
    private void Moving()
    {
        if(target == null) return;

        agent.Speed = moveSpeed;

        visual.ChangeState(UnitStateEnum.Move);
        visual.StartRotateMove();
        visual.Facing(target.Transform);
    }

    private void StopMoving()
    {
        agent.Speed = 0f;

        visual.StopRotateMove();
        visual.ChangeState(UnitStateEnum.Idle);
    }

    //======================== Attack ======================
    public void Attack()
    {
        if (target == null) return;

        if(attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        attack.FacingAttack();

        if (CanAttack())
        {
            PerformAttack();
        }
    }

    private bool CanAttack()
    {
        if (isAttacking) return false;
        return attackTimer <= 0f;
    }

    private void PerformAttack()
    {
        isAttacking = true;

        visual.ChangeState(UnitStateEnum.Attack);

        DealDamage();
        feedback.AttackForce(target.Transform);

        float finalCooldown = baseAttackCooldown / attackSpeedMultiplier;
        attackTimer = finalCooldown;

        // thời gian animation đánh
        Invoke(nameof(EndAttack), 0.3f);
    }

    private void EndAttack()
    {
        visual.ChangeState(UnitStateEnum.Idle);
        isAttacking = false;
    }

    public void SetAttackSpeedMultiplier(float value)
    {
        attackSpeedMultiplier = Mathf.Max(0.1f, value);
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
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(this, attackDamage);
            }
        }
    }

    //======================== Die ======================
    public void Die()
    {
        isDie = true;
        isAttacking = false;
        target = null;
        agent.Speed = 0f;

        StopAllCoroutines();
        visual.StopRotateMove();
        ChangeState(UnitStateEnum.Die);
    }

    //======================== Take Damage ======================
    public virtual void TakeDamage(UnitBase fromUnit, int damage)
    {
        if (isDie) return;

        buff.health.CurrentHealth -= damage;

        if(buff.health.CurrentHealth <= 0)
        {
            Die();

            // Effect
            void DoneKnockback()
            {
                visual.ChangeState(currentState);
            }

            Vector2 dir = (transform.position - fromUnit.transform.position).normalized;
            feedback.KnockbackFly(dir, DoneKnockback);
        }
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
    //======================== Combat ======================

    public void StartAction()
    {
        isAttacking = false;
    }

    public void SetTarget(ICombatEntity target)
    {
        this.target = target;

        if (target == null)
        {
            StopAllCoroutines();
            return;
        }

        followAgent.TargetAgent = target.Agent;
    }

    public ICombatEntity GetTarget()
    {
        return target;
    }

    public void Done()
    {
        StopAllCoroutines();
        StopMoving();

        ChangeState(UnitStateEnum.Idle);
        visual.ChangeState(UnitStateEnum.Idle);
    }

    #endregion

    #region Buff
    //======================== Buff ======================

    public void UpdateBuff()
    {
        if (isDie) return;

        buff.UpdateBuff();
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
