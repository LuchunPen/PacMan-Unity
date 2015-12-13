using System;

public enum CellType
{
    None,
    Tunnel,
    GhostHome,
    PacManSpawn,
    Wall, 
    Door,
    Point,
    Energizer,
    Cherry
}

public class PMNode
{
    private CellType _cType;
    public CellType CType
    {
        get { return _cType; }
        set { _cType = value; }
    }

    public bool IsObstacle
    {
        get { return _cType == CellType.Door || _cType == CellType.Wall; }
    }
}
