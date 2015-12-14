using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SpriteMapVisualizer: PacManMapVisualizer
{
    public TEDouble16 Walls;
    public TESingle Door;
    public TESingle FoodPoint;
    public TESingle FoodEnergizer;
    public TESingle FoodCherry;

    private TileExtractor[] _tileTypes;

    private GameObject[] _mapCellVis;

    void Awake()
    {
        _tileTypes = new TileExtractor[(int)CellType.size];

        _tileTypes[(int)CellType.Wall] = Walls;
        _tileTypes[(int)CellType.Door] = Door;
        _tileTypes[(int)CellType.Point] = FoodPoint;
        _tileTypes[(int)CellType.Energizer] = FoodEnergizer;
        _tileTypes[(int)CellType.Cherry] = FoodCherry;

    }

    public override void CreateOrRefreshVisualData()
    {
        Map2DCyclic<PMNode> map = GameManager.Instance.Map;
        if (map != null)
        {
            _mapCellVis = new GameObject[map.SizeX * map.SizeY];
            GameObject tileTemplate = null;

            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    int id = map.GetNodeID(x, y);
                    PMNode node = map[id];

                    TileExtractor te = _tileTypes[(int)node.CType];
                    DestroyTile(id);

                    if (te == null)
                    {
                        tileTemplate = null;
                        continue;
                    }
                    else if (te as TEDouble16 != null) { tileTemplate = GetTile(x, y, te as TEDouble16); }
                    else if (te as TESingle != null) { tileTemplate = GetTile(x, y, te as TESingle); }
                    
                    if (tileTemplate != null)
                    {
                        _mapCellVis[id] = Instantiate(tileTemplate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                        _mapCellVis[id].name = "Tile_x " + x + "_y " + y;
                        _mapCellVis[id].transform.SetParent(this.transform);
                    }
                }
            }
        }
    }

    private GameObject GetTile(int x, int y, TEDouble16 ted16)
    {
        Map2DCyclic<PMNode> map = GameManager.Instance.Map;

        CellType ctype = map[x, y].CType;
        int bestTile = 15;

        //Check corners
        if (map[x - 1, y - 1].CType != ctype) { bestTile &= ~(1 << 0); }
        if (map[x + 1, y - 1].CType != ctype) { bestTile &= ~(1 << 1); }
        if (map[x - 1, y + 1].CType != ctype) { bestTile &= ~(1 << 2); }
        if (map[x + 1, y + 1].CType != ctype) { bestTile &= ~(1 << 3); }

        //Check Edges
        if (map[x - 1, y].CType != ctype) { bestTile &= ~(1 << 0); bestTile &= ~(1 << 2); }
        if (map[x + 1, y].CType != ctype) { bestTile &= ~(1 << 1); bestTile &= ~(1 << 3); }
        if (map[x, y - 1].CType != ctype) { bestTile &= ~(1 << 0); bestTile &= ~(1 << 1); }
        if (map[x, y + 1].CType != ctype) { bestTile &= ~(1 << 2); bestTile &= ~(1 << 3); }

        return ted16.GetTileObject(bestTile);
    }
    private GameObject GetTile(int x, int y, TESingle tes)
    {
        return tes.GetTileObject(0);
    }
    private void DestroyTile(int id)
    {
        if (_mapCellVis[id] != null)
        {
            Destroy(_mapCellVis[id].gameObject);
        }
    }

    public override void RefreshVisualData(Vector3 node)
    {
        throw new NotImplementedException();
    }
    public override void RefreshVisualData(List<Vector3> nodes)
    {
        throw new NotImplementedException();
    }
    public override void DestroyVisualData()
    {
        throw new NotImplementedException();
    }
}
