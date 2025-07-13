using UnityEngine;

public class PromotionCollider : MonoBehaviour
{
    [SerializeField] private Piece promotionPiece;
    [SerializeField] private new Collider2D collider;

    public Piece GetPiece() => promotionPiece;
}