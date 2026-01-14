using DG.Tweening;
using System;
using UnityEngine;

public class UnitFeedback : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private UnitBase unit;
    [SerializeField] private Transform visual;
    [SerializeField] private Transform bodyTrans;

    [Header("Attack")]
    [SerializeField] private float attackMoveDistance = 0.3f; // Nhích tới bao xa
    [SerializeField] private float attackMoveTime = 0.1f;      // Thời gian nhích tới
    [SerializeField] private float returnTime = 0.1f;           // Thời gian lùi lại

    [Header("Knockback")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float maxHeight = 8f;
    [SerializeField] private float rotateAmount = 360f; // độ xoay
    [SerializeField] private bool rotateByDirection = false;

    private Vector3 originalPos;
    private Tween attackTween;

    public void AttackForce(Transform target)
    {
        // Hủy tween cũ nếu đang đánh
        attackTween?.Kill();

        originalPos = unit.transform.position;

        // Tính hướng tới target (2D top down)
        Vector3 dir = (target.position - unit.transform.position).normalized;

        // Vị trí nhích tới
        Vector3 attackPos = originalPos + dir * attackMoveDistance;

        attackTween = unit.transform
            .DOMove(attackPos, attackMoveTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                unit.transform.DOMove(originalPos, returnTime).SetEase(Ease.InQuad);
            });
    }

    public void KnockbackFly(Vector2 direction, Action onDone = null)
    {
        direction.Normalize();

        Vector2 startPos = unit.transform.position;
        Vector2 endPos = startPos + direction * distance;

        float elapsed = 0f;

        // Reset trước khi bay
        visual.localRotation = Quaternion.identity;

        DOTween.To(
            () => elapsed,
            x =>
            {
                elapsed = x;
                float t = elapsed / duration;

                // Move body X/Y
                unit.transform.position = Vector2.Lerp(startPos, endPos, t);

                // Parabola height
                float height = 4f * maxHeight * t * (1 - t);

                // Visual bay lên (Y offset)
                visual.localPosition = new Vector3(0, height, 0);
            },
            duration,
            duration
        )
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            visual.localPosition = Vector3.zero;
            visual.localScale = Vector3.one;
            unit.transform.position = endPos;

            onDone?.Invoke();
        });

        // ========== ROTATE ==========
        if (rotateByDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bodyTrans.DOLocalRotate(
                new Vector3(0, 0, angle),
                duration * 0.5f
            ).SetEase(Ease.OutBack);
        }
        else
        {
            bodyTrans.DOLocalRotate(
                new Vector3(0, 0, rotateAmount),
                duration,
                RotateMode.FastBeyond360
            ).SetEase(Ease.Linear);
        }
    }
}
