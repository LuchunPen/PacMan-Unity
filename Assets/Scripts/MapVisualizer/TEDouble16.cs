using UnityEngine;
using System;

[Serializable]
public class TEDouble16: TileExtractor
{
    private GameObject[] _tiles;

    public GameObject Horizontal;
    public GameObject Vertical;
    public GameObject CornerRightDown;
    public GameObject CornerRightUp;
    public GameObject CornerLeftUp;
    public GameObject CornerLeftDown;
    public GameObject DoubleCornerRULD;
    public GameObject DoubleCornerRDLU;

    private void Initialize()
    {
        _tiles = new GameObject[16];
        _tiles[0] = null;
        _tiles[1] = CornerLeftDown;
        _tiles[2] = CornerRightDown;
        _tiles[3] = Horizontal;
        _tiles[4] = CornerLeftUp;
        _tiles[5] = Vertical;
        _tiles[6] = DoubleCornerRDLU;
        _tiles[7] = CornerRightUp;

        _tiles[8] = CornerRightUp;
        _tiles[9] = DoubleCornerRULD;
        _tiles[10] = Vertical;
        _tiles[11] = CornerLeftUp;
        _tiles[12] = Horizontal;
        _tiles[13] = CornerRightDown;
        _tiles[14] = CornerLeftDown;
        _tiles[15] = null;
    }

    public override GameObject GetTileObject(int tilenum)
    {
        if (_tiles == null) { Initialize(); }
        if (tilenum < _tiles.Length) return _tiles[tilenum];
        else return null;
    }
}
