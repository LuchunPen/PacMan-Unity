using UnityEngine;

public class ClydeBeh: GhostBehaviour
{
    protected override bool _isFree
    {
        get
        {
            float persent = (float)GameManager.Instance.EatedPoints / (float)GameManager.Instance.TotalPoints;
            return persent > 0.3f;
        }
    }

    protected override void ChaseBehaviour()
    {
        GetNextPosition = GetMinNextPosition;
        SetBodyNormal();

        Vector3 newTarget = _pacman.position;
        float dist = Vector3.Distance(newTarget, transform.position);
        if (dist > 8) { _target = new Vector2(Mathf.FloorToInt(newTarget.x) + 0.5f, Mathf.FloorToInt(newTarget.y) + 0.5f); }
        else { _target = _scatterPosition; }

        _activeSpeedMod = _normalSpeedMod;
        MoveToNextPosition();
    }
}
