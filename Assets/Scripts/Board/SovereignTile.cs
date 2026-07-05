using UnityEngine;

public class SovereignTile : Tile
{
    [SerializeField] private SovereignTile partner; // other tile that shares same sovereign colour
    [SerializeField] private SovereignPieceSO sovereignPiece;
    [SerializeField] private Piece[] pieces;

    private void PawnPromoter_OnPawnPromotion(object sender, PawnPromoter.OnPawnPromotionEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public bool PartnerOccupied()
    {
        return partner.HasPiece();
    }
}