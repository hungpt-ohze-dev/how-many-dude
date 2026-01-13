using DG.Tweening;
using UnityEngine;

public class UnitFeedback : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private UnitBase unit;

    public float attackMoveDistance = 0.3f; // Nhích tới bao xa
    public float attackMoveTime = 0.1f;      // Thời gian nhích tới
    public float returnTime = 0.1f;           // Thời gian lùi lại

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
                unit.transform.DOMove(originalPos, returnTime)
                         .SetEase(Ease.InQuad);
            });
    }
}
