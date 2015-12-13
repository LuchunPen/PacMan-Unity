using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SpriteMapVisualizer: PacManMapVisualizer
{
    public GameObject FoodPoint;
    public GameObject FoodEnergizer;
    public GameObject FoodCherry;

    public GameObject Wall_Horizontal;
    public GameObject Wall_Vertical;
    public GameObject Wall_CornerRightDown;
    public GameObject Wall_CornerRightUp;
    public GameObject Wall_CornerLeftUp;
    public GameObject Wall_CornerLeftDown;
    public GameObject Wall_DoubleCornerRULD;
    public GameObject Wall_DoubleCornerRDLU;
    public GameObject Door;
    
    private GameObject[] _mapCellVis;
    private GameObject[] _tiles;

    void Awake()
    {
        _tiles = new GameObject[16];
        _tiles[0] = null;
        _tiles[1] = Wall_CornerLeftDown;
        _tiles[2] = Wall_CornerRightDown;
        _tiles[3] = Wall_Horizontal;
        _tiles[4] = Wall_CornerLeftUp;
        _tiles[5] = Wall_Vertical;
        _tiles[6] = Wall_DoubleCornerRDLU;
        _tiles[7] = Wall_CornerRightUp;

        _tiles[8] = Wall_CornerRightUp;
        _tiles[9] = Wall_DoubleCornerRULD;
        _tiles[10] = Wall_Vertical;
        _tiles[11] = Wall_CornerLeftUp;
        _tiles[12] = Wall_Horizontal;
        _tiles[13] = Wall_CornerRightDown;
        _tiles[14] = Wall_CornerLeftDown;
        _tiles[15] = null;

    }

    public override void CreateOrRefreshVisualData()
    {
        Map2DCyclic<PMNode> map = GameManager.Instance.Map;
        if (map != null)
        {
            _mapCellVis = new GameObject[map.SizeX * map.SizeY];
            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    int bestTile = 15;
                    int id = map.GetNodeID(x, y);
                    PMNode node = map[id];
                    if (node.CType == CellType.None) continue;

                    //Check corners
                    if (!map[x - 1, y - 1].IsObstacle) { bestTile &= ~(1 << 0); }
                    if (!map[x + 1, y - 1].IsObstacle) { bestTile &= ~(1 << 1); }
                    if (!map[x - 1, y + 1].IsObstacle) { bestTile &= ~(1 << 2); }
                    if (!map[x + 1, y + 1].IsObstacle) { bestTile &= ~(1 << 3); }

                    //Check Edges
                    if (!map[x - 1, y].IsObstacle) { bestTile &= ~(1 << 0); bestTile &= ~(1 << 2); }
                    if (!map[x + 1, y].IsObstacle) { bestTile &= ~(1 << 1); bestTile &= ~(1 << 3); }
                    if (!map[x, y - 1].IsObstacle) { bestTile &= ~(1 << 0); bestTile &= ~(1 << 1); }
                    if (!map[x, y + 1].IsObstacle) { bestTile &= ~(1 << 2); bestTile &= ~(1 << 3); }

                    if (_tiles[bestTile] != null)
                    {
                        _mapCellVis[id] = Instantiate(_tiles[bestTile], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                        _mapCellVis[id].name = "Tile_x " + x + "_y " + y;
                        _mapCellVis[id].transform.SetParent(this.transform);
                    }
                }
            }
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
