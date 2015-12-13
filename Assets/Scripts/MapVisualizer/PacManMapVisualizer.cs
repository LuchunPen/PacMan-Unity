using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class PacManMapVisualizer: MonoBehaviour, IMapVisualizer
{
    public abstract void CreateOrRefreshVisualData();
    public abstract void DestroyVisualData();
    public abstract void RefreshVisualData(List<Vector3> nodes);
    public abstract void RefreshVisualData(Vector3 node);
}
