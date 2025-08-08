using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CustomTools
{
    // used like CustomTools.DebugDisplayHashSetCoords("Legal moves:", _legalMoves);
    public static void DebugDisplayHashSetCoords(string preamble, HashSet<Vector2Int> coordinates)
    {
        List<string> coordStr = coordinates.Select(CoordinatesToString.Convert).ToList();

        coordStr.Sort();
    
        String debugString = coordStr.Aggregate(
            preamble, (current, coords) => current + coords + ", ");
        Debug.Log(debugString);
    }
}