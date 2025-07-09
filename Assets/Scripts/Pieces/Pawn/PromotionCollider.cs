
using UnityEngine;

public class PromotionCollider : MonoBehaviour
{
    [SerializeField] private Transform promotionPieceTransform;
    [SerializeField] private new Collider2D collider;

    public Transform GetPieceTransform() => promotionPieceTransform;
}