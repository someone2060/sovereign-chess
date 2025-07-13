
using System;
using UnityEngine;

public class PromotionColliderVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlightSprite;

    private bool _isActive;

    private void Awake()
    {
        highlightSprite.gameObject.SetActive(false);
    }

    private void Start()
    {
        PawnPromoter.Instance.OnShow += PawnPromoter_OnShow;
        PawnPromoterVisual.Instance.OnHover += PawnPromoterVisual_OnHover;
    }

    private void PawnPromoter_OnShow(object sender, EventArgs e)
    {
        highlightSprite.gameObject.SetActive(false);
    }

    private void PawnPromoterVisual_OnHover(object sender, PawnPromoterVisual.OnHoverEventArgs e)
    {
        if (Equals(e.promotionColliderVisual))
        {
            highlightSprite.gameObject.SetActive(true);
            return;
        }

        highlightSprite.gameObject.SetActive(false);
    }
}