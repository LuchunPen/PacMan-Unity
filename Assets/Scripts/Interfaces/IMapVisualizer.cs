using UnityEngine;
using System.Collections.Generic;

public interface IMapVisualizer
{
    void CreateOrRefreshVisualData();
    void RefreshVisualData(Vector3 node);
    void RefreshVisualData(List<Vector3> nodes);
    void DestroyVisualData();
}
