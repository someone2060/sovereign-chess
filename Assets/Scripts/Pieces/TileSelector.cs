using System;
using UnityEngine;

/* Uses InputHandler.cs to check if the player has interacted with any pieces */
public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputHandler.Instance.OnSelectPerformed += InputHandler_OnSelectPerformed;
    }

    // On selecting a tile with a piece on it, selection event is sent to piece
    private void InputHandler_OnSelectPerformed(object sender, EventArgs e)
    {
        Vector2 positionWorld = InputHandler.Instance.GetPositionWorld(Camera.main);

        Tile tile = GetTileOnWorld(positionWorld);
        if (tile is null) return;
        
        Debug.Log("Collision happened with tile at " + tile.GetCoordinates()); //TODO DEBUG
        
        if (!tile.HasPiece()) return;

        tile.GetPiece().Select();
    }

    public static Tile GetTileOnWorld(Vector2 position)
    {
        Collider2D collided = Physics2D.OverlapPoint(position);
        
        return collided?.GetComponent<Tile>();
    }
}
