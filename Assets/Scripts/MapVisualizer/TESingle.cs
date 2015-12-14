using UnityEngine;
using System;

[Serializable]
public class TESingle: TileExtractor
{
    public GameObject tile;

    public override GameObject GetTileObject(int tilenum)
    {
        return tile;
    }
}
