using UnityEngine;

/* Checks for collisions with Tile layer */
public class TileSelector : MonoBehaviour
{
    public static TileSelector Instance { get; private set; }
    
    private static LayerMask _layerMask;

    private void Awake()
    {
        Instance = this;
        _layerMask = 1 << LayerMask.NameToLayer("Tile");
    }

    public static Tile GetTileOnWorld(Vector2 position)
    {
        Collider2D collided = Physics2D.OverlapPoint(position, _layerMask);
        
        return collided?.GetComponent<Tile>();
    }
}
