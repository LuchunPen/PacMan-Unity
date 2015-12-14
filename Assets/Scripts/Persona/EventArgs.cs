using UnityEngine;
using System;

public class Collision2DArgs: EventArgs
{
    public GameObject collisionObject;

    public Collision2DArgs(Collider2D col)
    {
        collisionObject = col.gameObject;
    }
}
