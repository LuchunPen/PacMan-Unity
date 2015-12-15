using UnityEngine;
using System.Collections.Generic;
using System;

public enum GhostState
{
    AtHome = 0,
    Chase = 1,
    Scatter = 2,
    Frightened = 3,
    Dead = 4,
}

public class GhostBehaviour : Persona
{
    public static readonly float BlinkTime = 0.25f;
    public static readonly int GhostPoint = 200;

    public GameObject Body;
    public GameObject BodyGlow;

    protected Action PreviousAction;
    protected Action BehaviourAction;
    protected Action GetNextPosition;
    protected Direction _oppositeDirection;

    protected Transform _pacman;
    protected Vector3 _target;
    protected Vector3 _scatterPosition;
    protected Vector3 _homePosition;

    public bool IsFrattering
    {
        get { return BehaviourAction == FrightBehaviour; }
    }
    public bool IsDead
    {
        get { return BehaviourAction == DeadBehaviour; }
    }

    public bool _isFree = true;
    protected virtual bool isFree
    {
        get { return _isFree; }
    }

    void Start()
    {
        BehaviourAction = AtHomeBehaviour;
        _target = _myTrans.position;
    }

    public override void OnUpdate()
    {
        Debug.Log(BehaviourAction.Method.Name);
        if (BehaviourAction != null) { BehaviourAction(); }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DeadBehaviour();
        }
    }

    public void SetData(Transform pacman, Vector3 homePosition, Vector3 scatterPosition)
    {
        _pacman = pacman;
        _homePosition = homePosition;
        _scatterPosition = scatterPosition;
    }

    public override void OnDie()
    {
        _oppositeDirection = Direction.None;
        BehaviourAction = DeadBehaviour;
    }

    public void GhostBehaviourEventHandler(object sender, LevelWaveArgs args)
    {
        if (BehaviourAction == AtHomeBehaviour || BehaviourAction == DeadBehaviour) return;

        if (args.State == GhostState.Frightened)
        {
            PreviousAction = BehaviourAction;
            BehaviourAction = FrightBehaviour;
            _oppositeDirection = Direction.None;
        }
        else if (args.State == GhostState.Chase)
        {
            BehaviourAction = ChaseBehaviour;
        }
        else if (args.State == GhostState.Scatter)
        {
            _oppositeDirection = Direction.None;
            BehaviourAction = ScatterBehaviour;
        }
    }

    protected void SetDirection(Direction dir, int ix, int iy)
    {
        if (dir == Direction.None) return;

        _direction = dir;
        _anim.SetInteger("Direction", (int)_direction);

        if (dir == Direction.Left)
        {
            _oppositeDirection = Direction.Right;
            _nextPosition = new Vector3(ix - 1 + 0.5f, iy + 0.5f);
        }
        else if (dir == Direction.Right)
        {
            _oppositeDirection = Direction.Left;
            _nextPosition = new Vector3(ix + 1 + 0.5f, iy + 0.5f);
        }
        else if (dir == Direction.Down)
        {
            _oppositeDirection = Direction.Up;
            _nextPosition = new Vector3(ix + 0.5f, iy - 1 + 0.5f);
        }
        else if (dir == Direction.Up)
        {
            _oppositeDirection = Direction.Down;
            _nextPosition = new Vector3(ix + 0.5f, iy + 1 + 0.5f);
        }
    }

    protected void GetRandomNextPosition()
    {
        int ix = Mathf.FloorToInt(_myTrans.position.x);
        int iy = Mathf.FloorToInt(_myTrans.position.y);

        List<Direction> _potencialDir = new List<Direction>();
        if (!GameManager.Instance.Map[ix - 1, iy].IsObstacle && _oppositeDirection != Direction.Left) { _potencialDir.Add(Direction.Left); }
        if (!GameManager.Instance.Map[ix + 1, iy].IsObstacle && _oppositeDirection != Direction.Right) { _potencialDir.Add(Direction.Right); }
        if (!GameManager.Instance.Map[ix, iy - 1].IsObstacle && _oppositeDirection != Direction.Down) { _potencialDir.Add(Direction.Down); }
        if (!GameManager.Instance.Map[ix, iy + 1].IsObstacle && _oppositeDirection != Direction.Up) { _potencialDir.Add(Direction.Up); }

        SetDirection((Direction)UnityEngine.Random.Range(0, _potencialDir.Count), ix, iy);
    }

    protected void GetMinNextPosition()
    {
        int ix = Mathf.FloorToInt(_myTrans.position.x);
        int iy = Mathf.FloorToInt(_myTrans.position.y);

        float dist = float.MaxValue;
        Direction _potencialDir = Direction.None;

        if (!GameManager.Instance.Map[ix - 1, iy].IsObstacle && _oppositeDirection != Direction.Left)
        { _potencialDir = Direction.Left; dist = Vector3.Distance(new Vector3(ix - 1, iy), _target); }
        if (!GameManager.Instance.Map[ix + 1, iy].IsObstacle && _oppositeDirection != Direction.Right)
        {
            float newDist = Vector3.Distance(new Vector3(ix + 1, iy), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Right; }
        }
        if (!GameManager.Instance.Map[ix, iy - 1].IsObstacle && _oppositeDirection != Direction.Down)
        {
            float newDist = Vector3.Distance(new Vector3(ix, iy - 1), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Down; }
        }
        if (!GameManager.Instance.Map[ix, iy + 1].IsObstacle && _oppositeDirection != Direction.Up)
        {
            float newDist = Vector3.Distance(new Vector3(ix, iy + 1), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Up; }
        }
        SetDirection(_potencialDir, ix, iy);
    }

    protected void GetMinNextPositionHome()
    {
        int ix = Mathf.FloorToInt(_myTrans.position.x);
        int iy = Mathf.FloorToInt(_myTrans.position.y);

        float dist = float.MaxValue;
        Direction _potencialDir = Direction.None;

        if (GameManager.Instance.Map[ix - 1, iy].CType != CellType.Wall && _oppositeDirection != Direction.Left)
        { _potencialDir = Direction.Left; dist = Vector3.Distance(new Vector3(ix - 1, iy), _target); }
        if (GameManager.Instance.Map[ix + 1, iy].CType != CellType.Wall && _oppositeDirection != Direction.Right)
        {
            float newDist = Vector3.Distance(new Vector3(ix + 1, iy), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Right; }
        }
        if (GameManager.Instance.Map[ix, iy - 1].CType != CellType.Wall && _oppositeDirection != Direction.Down)
        {
            float newDist = Vector3.Distance(new Vector3(ix, iy - 1), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Down; }
        }
        if (GameManager.Instance.Map[ix, iy + 1].CType != CellType.Wall && _oppositeDirection != Direction.Up)
        {
            float newDist = Vector3.Distance(new Vector3(ix, iy + 1), _target);
            if (newDist < dist) { dist = newDist; _potencialDir = Direction.Up; }
        }
        SetDirection(_potencialDir, ix, iy);
    }

    protected override void MoveToNextPosition()
    {
        Vector3 position = _myTrans.position;
        AutoCorrectPosition(ref position, ref _nextPosition);
        float dist = Vector3.Distance(position, _nextPosition);
        if (dist < 0.1f || dist > 1.3f)
        {
            GetNextPosition();
        }
        _myTrans.position = Vector3.MoveTowards(position, _nextPosition, Time.deltaTime * defaultspeed * 0.5f);

    }

    #region Behaviours

    protected virtual void ChaseBehaviour()
    {
        GetNextPosition = GetMinNextPosition;
        SetBodyNormal();

        _target = _pacman.position;
        MoveToNextPosition();
    }

    protected virtual void ScatterBehaviour()
    {
        GetNextPosition = GetMinNextPosition;
        SetBodyNormal();
        _target = _scatterPosition;
        MoveToNextPosition();
    }

    protected void FrightBehaviour()
    {
        GetNextPosition = GetRandomNextPosition;

        float reducetime = GameManager.Instance.FrightTime;
        if (reducetime > 1.5f)
        {
            SetBodyFright();
        }
        else if (reducetime < 1.5f && reducetime > 0)
        {
            BlinkBody(reducetime);
        }
        else if (reducetime <= 0)
        {
            BehaviourAction = PreviousAction;
            PreviousAction = null;
        }
        MoveToNextPosition();
    }

    protected void DeadBehaviour()
    {
        GetNextPosition = GetMinNextPositionHome;
        SetBodyDead();
        _target = _homePosition;
        Vector3 pos = _myTrans.position;

        float dist = Vector3.Distance(_target, pos);
        if (dist < 0.2f)
        {
            BehaviourAction = AtHomeBehaviour;
        }
        MoveToNextPosition();
    }

    protected void AtHomeBehaviour()
    {
        GetNextPosition = GetMinNextPositionHome;
        SetBodyNormal();
        Vector3 pos = _myTrans.position;
        _oppositeDirection = Direction.None;

        if (isFree)
        {
            _target = GameManager.Instance.GetHomeEnter(pos);
            float dist = Vector3.Distance(_target, pos);
            if (dist < 0.2f)
            {
                BehaviourAction = ChaseBehaviour;
                return;
            }
        }
        else
        {
            float dist = Vector3.Distance(_target, pos);
            if (dist < 0.2f)
            {
                _target = GameManager.Instance.GetGhostSpawn;
                GetNextPosition();
            }
        }
        MoveToNextPosition();
    }

    #endregion Behaviours

    #region Visual
    protected void BlinkBody(float time)
    {
        float bltime = Mathf.CeilToInt(time / BlinkTime);
        if (bltime % 2 != 0) { SetBodyNormal(); }
        else SetBodyFright();
    }
    protected void SetBodyNormal()
    {
        Body.SetActive(false);
        BodyGlow.SetActive(true);
    }
    protected void SetBodyFright()
    {
        Body.SetActive(true);
        BodyGlow.SetActive(false);
    }
    protected void SetBodyDead()
    {
        Body.SetActive(false);
        BodyGlow.SetActive(false);
    }
    #endregion Visual
}
