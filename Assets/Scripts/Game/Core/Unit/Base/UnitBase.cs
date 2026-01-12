using System.Collections;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable
{
    [Header("Info")]
    public UnitStateEnum currentState;

    [Header("Component")]
    [SerializeField] private UnitStateVisual stateVisual;

    [Header("Value")]
    public float moveSpeed = 10f;
    public float stopDistance = 0.2f;

    [Header("Attack")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 0.2f;


    private bool isMoving;
    private bool isAttacking;
    private bool isDie;

    private UnitBase target;

    //======================== Setup ======================
    public virtual void Init()
    {
        stateVisual.Set();
    }

    #region Behavior
    //======================== Idle ======================
    public void Idle()
    {
        stateVisual.ChangeState(currentState);
    }

    //======================== Move ======================
    public void MoveToTarget()
    {
        if (isMoving) return;

        StartCoroutine(MoveToTargetCoroutine(target.transform));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target)
    {
        isMoving = true;

        stateVisual.ChangeState(UnitStateEnum.Move);
        stateVisual.StartRotateMove();
        stateVisual.Facing(target);

        while (target != null)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance <= stopDistance)
                break;

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * direction;

            yield return null; // chờ frame tiếp theo
        }

        // Reach target
        isMoving = false;

        stateVisual.StopRotateMove();
        stateVisual.ChangeState(UnitStateEnum.Idle);
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
        stateVisual.ChangeState(UnitStateEnum.Attack);
        DealDamage();

        yield return new WaitForSeconds(attackCooldown);
        stateVisual.ChangeState(UnitStateEnum.Idle);

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

        StopAllCoroutines();
        stateVisual.ChangeState(UnitStateEnum.Die);
    }

    //======================== Take Damage ======================
    public virtual void TakeDamage(int damage)
    {
        Debug.Log("AAA");
    }

    #endregion

    #region Update State

    //======================== Update ======================
    public void SetTarget(UnitBase target)
    {
        this.target = target;

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

        float dist = Vector3.Distance(transform.position, target.transform.position);

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
            ChangeState(UnitStateEnum.Idle);
            return;
        }

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist <= stopDistance)
        {
            ChangeState(UnitStateEnum.Attack);
            return;
        }

        // Move to target
        MoveToTarget();
    }

    void UpdateAttack()
    {
        if (target == null)
        {
            ChangeState(UnitStateEnum.Idle);
            return;
        }

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > stopDistance)
        {
            ChangeState(UnitStateEnum.Move);
            return;
        }

        // Logic attack
        Attack();
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
