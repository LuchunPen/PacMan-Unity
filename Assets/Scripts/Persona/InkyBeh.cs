using UnityEngine;

public class InkyBeh: GhostBehaviour
{
    protected override bool _isFree
    {
        get { return GameManager.Instance.EatedPoints > 30; }
    }

    private Transform _blinky;
    public void SetData(Transform pacman, Transform blinky, Vector3 homePosition, Vector3 scatterPosition)
    {
        base.SetData(pacman, homePosition, scatterPosition);
        _blinky = blinky;
    }

    protected override void ChaseBehaviour()
    {
        GetNextPosition = GetMinNextPosition;
        SetBodyNormal();

        Vector3 playerpos = _pacman.position;
        Vector3 blinkipos = _blinky.position;

        Vector3 pos = playerpos - blinkipos;
        _target = new Vector2(Mathf.FloorToInt(playerpos.x + pos.x) + 0.5f, Mathf.FloorToInt(playerpos.y + pos.y) + 0.5f);

        _activeSpeedMod = _normalSpeedMod;
        MoveToNextPosition();
    }
}
