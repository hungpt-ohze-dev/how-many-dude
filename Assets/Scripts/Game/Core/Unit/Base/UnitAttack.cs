using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private SpriteRenderer bodyRender;

    public void FacingAttack()
    {
        float xScale = bodyRender.flipX ? -1f : 1f;
        transform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
