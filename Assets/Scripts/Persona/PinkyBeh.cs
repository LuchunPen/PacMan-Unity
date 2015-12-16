using UnityEngine;
using System.Collections;

public class PinkyBeh: GhostBehaviour
{
    private Persona _targetPersona;
    
    protected override void ChaseBehaviour()
    {
        GetNextPosition = GetMinNextPosition;
        SetBodyNormal();

        if (_targetPersona == null)
        {
            _targetPersona = _pacman.GetComponent<Persona>();
        }

        _target = new Vector2(Mathf.FloorToInt(_pacman.position.x) + 0.5f, Mathf.FloorToInt(_pacman.position.y) + 0.5f);
        Direction targetDirection = _targetPersona.ActualDirection;

        switch (targetDirection)
        {
            case Direction.Left: _target.x -= 4; break;
            case Direction.Right: _target.x += 4; break;
            case Direction.Up: _target.y += 4; break;
            case Direction.Down: _target.y -= 4; break;
        }

        _activeSpeedMod = _normalSpeedMod;
        MoveToNextPosition();

    }
}
