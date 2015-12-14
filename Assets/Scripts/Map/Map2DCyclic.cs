using UnityEngine;
using System;

public class Map2DCyclic<TCell>: IMap2D<TCell> where TCell: class, new()
{
    private TCell[] _mapNodes;

    private int _sizeX;
    private int _sizeY;
    private int _mapSize;

    public TCell this[int id]
    {
        get { return _mapNodes[GetMapID(id)]; }
        set { if (value != null) { _mapNodes[GetMapID(id)] = value; } }
    }
    public TCell this[int posX, int posY]
    {
        get { return _mapNodes[GetNodeID(posX, posY)]; }
        set { if (value != null) { _mapNodes[GetNodeID(posX, posY)] = value; } }
    }

    public int SizeX
    {
        get { return _sizeX; }
    }
    public int SizeY
    {
        get { return _sizeY; }
    }

    public Map2DCyclic(int sizeX, int sizeY)
    {
        _sizeX = sizeX; if (_sizeX < 1) { _sizeX = 1; }
        _sizeY = sizeY; if (_sizeY < 1) { _sizeY = 1; }

        _mapSize = _sizeX * _sizeY;
        _mapNodes = new TCell[_mapSize];
        for (int i = 0; i < _mapNodes.Length; i++)
        {
            _mapNodes[i] = new TCell();
        }
    }
    public int GetNodeID(int posX, int posY)
    {
        int dx = posX % _sizeX; if (dx < 0) dx = _sizeX + dx;
        int dy = posY % _sizeY; if (dy < 0) dy = _sizeY + dy;
        return dx * _sizeY + dy;
    }
    public void GetNodePos(int nodeID, out int posX, out int posY)
    {
        throw new NotImplementedException();
    }

    private int GetMapID(int id)
    {
        int mapid = id % _mapSize;
        if (mapid < 0) mapid += _mapSize;
        return mapid;
    }
}
