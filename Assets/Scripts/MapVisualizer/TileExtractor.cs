using UnityEngine;

[System.Serializable]
public abstract class TileExtractor
{
    public abstract GameObject GetTileObject(int tilenum);
}
