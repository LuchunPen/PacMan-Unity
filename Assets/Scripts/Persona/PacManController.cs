using UnityEngine;
using System.Collections;
using System;

public class PacManController: Persona
{
    public EventHandler<Collision2DArgs> CollisionEvent;

    private float _speedModNormal;
    private float _speedModFright;

    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }

    protected override void Awake()
    {
        base.Awake();
        PersonaSpeed speedTable = ClassicLevelTable.GetSpeedTable(GameManager.Instance.Level);
        _speedModNormal = speedTable.PacManSpeedNormal;
        _speedModFright = speedTable.PacManSpeedFright;

        CheckDirection(Direction.Left, _myTrans.position);
    }

    public override void OnUpdate ()
    {
        if (_direction != Direction.None) MoveToNextPosition();

        if (!IsDead)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckDirection(Direction.Left, _myTrans.position);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                CheckDirection(Direction.Right, _myTrans.position);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                CheckDirection(Direction.Up, _myTrans.position);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CheckDirection(Direction.Down, _myTrans.position);
            }
        }
    }

    private void CheckDirection(Direction dir, Vector3 position)
    {
        if (GameManager.Instance.Map == null) return;
        if (dir == Direction.None) return;

        int ix = Mathf.FloorToInt(position.x);
        int iy = Mathf.FloorToInt(position.y);

        float dy = position.y - iy;
        float dx = position.x - ix;

        if (dir == Direction.Left && !GameManager.Instance.Map[ix - 1, iy].IsObstacle)
        {
            if (dy < 0.25f || dy > 0.75f) return;
            _nextPosition = new Vector3(ix - 1 + 0.5f, iy + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Right && !GameManager.Instance.Map[ix + 1, iy].IsObstacle)
        {
            if (dy < 0.25f || dy > 0.75f) return;
            _nextPosition = new Vector3(ix + 1 + 0.5f, iy + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Down && !GameManager.Instance.Map[ix, iy - 1].IsObstacle)
        {
            if (dx < 0.25f || dx > 0.75f) return;
            _nextPosition = new Vector3(ix + 0.5f, iy - 1 + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Up && !GameManager.Instance.Map[ix, iy + 1].IsObstacle)
        {
            if (dx < 0.25f || dx > 0.75f) return;
            _nextPosition = new Vector3(ix + 0.5f, iy + 1 + 0.5f);
            _direction = dir;
        }
        _anim.SetInteger("Direction", (int)_direction);
    }

    protected override void MoveToNextPosition()
    {
        if (GameManager.Instance.FrightTime > 0) { _activeSpeedMod = _speedModFright; }
        else { _activeSpeedMod = _speedModNormal; }

        Vector3 position = _myTrans.position;
        AutoCorrectPosition(ref position, ref _nextPosition);

        _myTrans.position = Vector3.MoveTowards(position, _nextPosition, Time.deltaTime * defaultspeed * _activeSpeedMod);

        int ix = Mathf.FloorToInt(position.x);
        int iy = Mathf.FloorToInt(position.y);

        if ((position.y - iy > 0.45f || position.y - iy < 0.55f)
            && (position.x - ix > 0.45f || position.x - ix < 0.55f))
        {
            CheckDirection(_direction, _myTrans.position);
        }
    }

    public override void OnDie()
    {
        _anim.SetBool("IsDead", true);
        _direction = Direction.None;
        _isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CollisionEvent != null) { CollisionEvent(this, new Collision2DArgs(other, this.gameObject)); }
    }
}
