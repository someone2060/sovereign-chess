using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private SpriteRenderer tileVisual;
    [SerializeField] private Vector2Int coordinates;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileVisual.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
