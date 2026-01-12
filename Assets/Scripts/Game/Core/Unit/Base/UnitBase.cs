using System.Collections;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private UnitState state;

    [Header("Value")]
    public float moveSpeed = 10f;
    public float stopDistance = 0.2f;

    private bool isAttacking;
    private bool isDie;

    private Coroutine moveCoroutine;

    //======================== Setup ======================
    public virtual void Init()
    {

    }

    #region Behavior

    //======================== Move ======================
    public void MoveTo(Transform target)
    {
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target)
    {
        state.ChangeState(UnitStateEnum.Move); // set state ban đầu
        state.StartRotateMove();
        state.Facing(target);

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
        state.StopRotateMove();
        state.ChangeState(UnitStateEnum.Idle);
        moveCoroutine = null;
    }

    public void StopMoving()
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }

    //======================== Attack ======================
    public void Attack()
    {
        if (isDie) return;
        if (isAttacking) return;

        StartCoroutine(AttackIEnum());
    }

    private IEnumerator AttackIEnum()
    {
        isAttacking = true;
        state.ChangeState(UnitStateEnum.Attack);

        yield return new WaitForSeconds(0.2f);

        isAttacking = false;
        state.ChangeState(UnitStateEnum.Idle);
    }

    //======================== Die ======================
    public void Die()
    {
        isDie = true;

        StopAllCoroutines();
        state.ChangeState(UnitStateEnum.Die);
    }

    #endregion
}
