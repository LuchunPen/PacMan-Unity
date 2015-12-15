using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Persona: MonoBehaviour
{
    public static float defaultspeed = 5;
    protected float _activeSpeedMod;

    protected Direction _direction = Direction.None;
    protected Vector3 _nextPosition;

    protected Animator _anim;
    protected Transform _myTrans;
    protected Collider2D _col;

    protected virtual void Awake()
    {
        _myTrans = this.transform;
        _anim = this.GetComponent<Animator>();
        _col = this.GetComponent<Collider2D>();
    }

    public abstract void OnUpdate();
    public abstract void OnDie();

    protected abstract void MoveToNextPosition();

    protected void AutoCorrectPosition(ref Vector3 position, ref Vector3 nextPosition)
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
}
