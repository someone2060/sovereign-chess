using UnityEngine;

public class SovereignTile : Tile
{
    [SerializeField] private SovereignTile partner; // other tile that shares same sovereign colour
    [SerializeField] private SovereignPieceSO sovereignPiece;
    [SerializeField] private Piece[] pieces;

    private void Start()
    {
        tileVisual.color = sovereignPiece.color;
    }

    public bool PartnerOccupied()
    {
        return partner.HasPiece();
    }
}