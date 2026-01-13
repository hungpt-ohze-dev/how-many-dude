using DG.Tweening;
using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private SpriteRenderer spriteRen;
    [SerializeField] private Transform renderTrans;

    [Header("Visual")]
    public Sprite idleSprite;
    public Sprite attackSprite;
    public Sprite dieSprite;

    [Header("Move Animation")]
    [SerializeField] private float rotateAngle = 15f;
    [SerializeField] private float rotateDuration = 0.25f;

    [SerializeField] private float jumpHeight = 0.2f;
    [SerializeField] private float jumpDuration = 0.25f;

    // Private
    private Tween rotateTween;
    private Tween jumpTween;

    private UnitStateEnum currentState;
    private float startY;

    public void Set()
    {
        startY = renderTrans.localPosition.y;
    }

    // ========================= STATE ===========================
    #region State
    public void ChangeState(UnitStateEnum newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case UnitStateEnum.Idle:
                IdleState();
                break;

            case UnitStateEnum.Move:
                MoveState();
                break;

            case UnitStateEnum.Attack:
                AttackState();
                break;

            case UnitStateEnum.Die:
                DieState();
                break;
        }
    }

    private void IdleState()
    {
        spriteRen.sprite = idleSprite;
    }

    private void MoveState()
    {
        spriteRen.sprite = idleSprite;
    }

    private void AttackState()
    {
        spriteRen.sprite = attackSprite;
    }

    private void DieState()
    {
        spriteRen.sprite = dieSprite;
    }

    #endregion

    public void Facing(Vector3 direction)
    {
        spriteRen.flipX = direction.x <= 0f;
    }

    public void Facing(Transform target)
    {
        Vector3 dir = target.position - this.transform.position;
        Facing(dir.normalized);
    }

    // Anim
    public void StartRotateMove()
    {
        if (rotateTween != null && rotateTween.IsActive())
            return;

        // Rotate
        rotateTween = renderTrans
            .DOLocalRotate(
                new Vector3(0, 0, rotateAngle),
                rotateDuration
            )
            .From(new Vector3(0, 0, -rotateAngle))
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // Jump
        jumpTween = renderTrans
            .DOLocalMoveY(startY + jumpHeight, jumpDuration)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopRotateMove()
    {
        if (rotateTween != null)
        {
            rotateTween.Kill();
            rotateTween = null;
        }

        if (jumpTween != null)
        {
            jumpTween.Kill();
            jumpTween = null;
        }

        // Trả về trạng thái ban đầu mượt
        renderTrans.DOLocalRotate(Vector3.zero, 0.15f);
        renderTrans.DOLocalMoveY(startY, 0.15f);
    }
}
