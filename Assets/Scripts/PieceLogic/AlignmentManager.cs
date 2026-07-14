using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlignmentManager : MonoBehaviour
{
    [SerializeField] private SovereignPieceSO[] sovereignPieces; // unneeded to be in correct order
    private HashSet<Piece>[] _pieces; // don't need to manually set
    
    public static AlignmentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _pieces = new HashSet<Piece>[sovereignPieces.Length];
        for (int i = 0; i < sovereignPieces.Length; i++)
        {
            _pieces[i] = new HashSet<Piece>();
        }
    }

    private void Start()
    {
        PieceMover.Instance.OnPieceSelected += PieceMoverOnPieceSelected; // for debugging
    }

    private void PieceMoverOnPieceSelected(object sender, PieceMover.OnPieceEventArgs e)
    {
        for (int i = 0; i < sovereignPieces.Length; i++)
        {
            var str = "HashSet " + i + ": ";
            List<string> coords = _pieces[i].Select(piece => CustomTools.CoordinatesToString(piece.GetTile().GetCoordinates())).ToList();
            coords.Sort();
            str = coords.Aggregate(str, (current, coord) => current + (coord + ", "));
            str += " (length " + coords.Count + ")"; 
            Debug.Log(str);
        }
        Debug.Log("selected piece is on " + CustomTools.CoordinatesToString(e.piece.GetTile().GetCoordinates()));
    }

    public void ChangeAlignment(SovereignPieceSO sovereignPiece, Piece.Alignment newAlignment)
    {
        foreach (var piece in _pieces[sovereignPiece.id])
        {
            piece.SetAlignment(newAlignment);
        }
    }

    public void AddToPieces(Piece piece)
    {
        if (piece is null) return;
        Debug.Log("piece added");
        _pieces[piece.GetSovereignPiece().id].Add(piece);
    }

    public bool RemoveFromPieces(Piece piece)
    {
        if (piece is null) return false;
        Debug.Log("piece removed");
        return _pieces[piece.GetSovereignPiece().id].Remove(piece);
    }
}
