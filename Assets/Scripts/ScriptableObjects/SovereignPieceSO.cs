using UnityEngine;

[CreateAssetMenu(fileName = "SovereignPiece", menuName = "ScriptableObjects/Sovereign Piece")]
public class SovereignPieceSO : ScriptableObject
{
    public int id;
    public Sprite kingSprite;
    public Sprite queenSprite;
    public Sprite bishopSprite;
    public Sprite knightSprite;
    public Sprite rookSprite;
    public Sprite pawnSprite;
    public Color color;
}
