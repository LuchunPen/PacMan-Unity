using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Persona: MonoBehaviour
{
    public static float defaultspeed = 5;
    protected float _activeSpeedMod;

    protected Direction _direction = Direction.None;

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
}
