using UnityEngine;
using System.Collections;
using System;

public class PacManController: Persona
{
    public EventHandler<Collision2DArgs> CollisionEvent;
    public float speedMod = 1;
    public float speedModFright;

    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }
    public Vector3 _nextPosition;

    protected override void Awake()
    {
        base.Awake();
        _activeSpeedMod = speedMod;
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnDie();
            }
        }
    }

    private void CheckDirection(Direction dir, Vector3 position)
    {
        if (GameManager.Instance.Map == null) return;
        if (dir == Direction.None) return;

        int ix = Mathf.FloorToInt(position.x);
        int iy = Mathf.FloorToInt(position.y);

        if (dir == Direction.Left && !GameManager.Instance.Map[ix - 1, iy].IsObstacle)
        {
            if (position.y - iy < 0.25f || position.y - iy > 0.75f) return;
            _nextPosition = new Vector3(ix - 1 + 0.5f, iy + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Right && !GameManager.Instance.Map[ix + 1, iy].IsObstacle)
        {
            if (position.y - iy < 0.25f || position.y - iy > 0.75f) return;
            _nextPosition = new Vector3(ix + 1 + 0.5f, iy + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Down && !GameManager.Instance.Map[ix, iy - 1].IsObstacle)
        {
            if (position.x - ix < 0.25f || position.x - ix > 0.75f) return;
            _nextPosition = new Vector3(ix + 0.5f, iy - 1 + 0.5f);
            _direction = dir;
        }

        else if (dir == Direction.Up && !GameManager.Instance.Map[ix, iy + 1].IsObstacle)
        {
            if (position.x - ix < 0.25f || position.x - ix > 0.75f) return;
            _nextPosition = new Vector3(ix + 0.5f, iy + 1 + 0.5f);
            _direction = dir;
        }
        _anim.SetInteger("Direction", (int)_direction);
    }

    private void MoveToNextPosition()
    {
        Vector3 position = _myTrans.position;
        AutoCorrectPosition(ref position, ref _nextPosition);

        _myTrans.position = Vector3.MoveTowards(position, _nextPosition, Time.deltaTime * defaultspeed * _activeSpeedMod);

        int ix = Mathf.FloorToInt(position.x);
        int iy = Mathf.FloorToInt(position.y);

        if ((position.y - iy > 0.4f || position.y - iy < 0.6f)
            && (position.x - ix > 0.4f || position.x - ix < 0.6f))
        {
            //gameManager.EatFood(px, py);
            CheckDirection(_direction, _myTrans.position);
        }
    }

    private void AutoCorrectPosition(ref Vector3 position, ref Vector3 nextPosition)
    {
        int ix = Mathf.FloorToInt(_myTrans.position.x);
        int iy = Mathf.FloorToInt(_myTrans.position.y);

        if (ix < 0)
        {
            position.x = GameManager.Instance.Map.SizeX;
            nextPosition.x = position.x - 0.5f;
        }
        else if (ix > GameManager.Instance.Map.SizeX)
        {
            position.x = 0; nextPosition.x = 0.5f;
        }

        if (iy < 0)
        {
            position.y = GameManager.Instance.Map.SizeY;
            nextPosition.y = position.y - 0.5f;
        }
        else if (iy > GameManager.Instance.Map.SizeY)
        {
            position.y = 0; nextPosition.y = 0.5f;
        }
    }

    private void OnDie()
    {
        _anim.SetBool("IsDead", true);
        _direction = Direction.None;
        _isDead = true;
        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CollisionEvent != null) { CollisionEvent(this, new Collision2DArgs(other)); }
    }
}
