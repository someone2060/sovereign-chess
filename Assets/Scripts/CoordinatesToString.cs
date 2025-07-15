using System;
using UnityEngine;

public class CoordinatesToString : MonoBehaviour
{
    public static CoordinatesToString Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static String Convert(Vector2Int vector2Int)
    {
        return (char)(vector2Int.x + 97) + (vector2Int.y + 1).ToString();
    }
}