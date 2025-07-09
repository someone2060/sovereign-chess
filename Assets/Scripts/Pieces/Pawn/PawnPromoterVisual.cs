using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PawnPromoterVisual : MonoBehaviour
{
    public static PawnPromoterVisual Instance { get; private set; }
    
    [SerializeField] private SpriteRenderer queenSprite;
    [SerializeField] private SpriteRenderer rookSprite;
    [SerializeField] private SpriteRenderer bishopSprite;
    [SerializeField] private SpriteRenderer kingSprite;
    [SerializeField] private SpriteRenderer knightSprite;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
        PawnPromoter.Instance.OnShow += PawnPromoter_OnShow;
        PawnPromoter.Instance.OnHide += PawnPromoter_OnHide;
    }

    private void PawnPromoter_OnHide(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void PawnPromoter_OnShow(object sender, EventArgs e)
    {
        SovereignPieceSO sovereignPieceSO = PawnPromoter.Instance.GetSovereignPiece();
        queenSprite.sprite = sovereignPieceSO.queenSprite;
        rookSprite.sprite = sovereignPieceSO.rookSprite;
        bishopSprite.sprite = sovereignPieceSO.bishopSprite;
        kingSprite.sprite = sovereignPieceSO.kingSprite;
        knightSprite.sprite = sovereignPieceSO.knightSprite;

        gameObject.SetActive(true);
    }
    
    
}
