using UnityEngine;
using System.Collections;

public interface IMap2D<TCell>
{
    TCell this[int id] { get; set; }
    TCell this[int posX, int posY] { get; set; }

    int SizeX { get; }
    int SizeY { get; }

    int GetNodeID(int posX, int posY);
    void GetNodePos(int nodeID, out int posX, out int posY);
}
