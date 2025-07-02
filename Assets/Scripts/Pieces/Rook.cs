using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Rook : Piece
{
    private bool _pSelected;
    
    protected new void Start()
    {
        base.Start();
        spriteRenderer.sprite = sovereignPiece.rookSprite;
        _pSelected = false;
    }

    protected new void Update()
    {
        base.Update();
        if (_selected && !_pSelected) //TODO DEBUG
        {
            HashSet<Vector2Int> legalMoves = LegalMoves();
            List<string> coordStr = legalMoves.Select(legalMove => Board.Instance.GetTile(legalMove).GetCoordinates().ToString()).ToList();
            coordStr.Sort();
            
            String debugString = coordStr.Aggregate("Legal moves: ", (current, coords) => current + coords + ", ");
            Debug.Log(debugString);
        }

        _pSelected = _selected;
    }
    
    public override HashSet<Vector2Int> LegalMoves()
    {
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        
        Vector2Int coordsVec = GetCoordinates().GetVector2Int();
        
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.right));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.left));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.up));
        legalMoves.AddRange(SearchInDirection(coordsVec, Vector2Int.down));
        return legalMoves;
    }

    // Extends 8 tiles in search direction until colliding with another piece or reaching end of board
    private HashSet<Vector2Int> SearchInDirection(Vector2Int coordsVec, Vector2Int dir)
    {
        Vector2Int incrementVec = Vector2Int.zero;
        HashSet<Vector2Int> legalMoves = new HashSet<Vector2Int>();
        for (int i = 0; i < 8; i++)
        {
            incrementVec += dir;
            Tile testTile = Board.Instance.GetTile(coordsVec + incrementVec);
            if (testTile is null) break; // end of board, searching illegal tile
            if (!testTile.HasPiece()) // no piece on checked tile
            {
                legalMoves.Add(coordsVec + incrementVec);
                continue;
            }

            if (testTile.GetPiece().CanBeCaptured(alignment)) // piece on checked tile can be captured
            {
                legalMoves.Add(coordsVec + incrementVec);
            }

            // piece on checked tile can't be captured
            break;
        }
        return legalMoves;
    }
}
